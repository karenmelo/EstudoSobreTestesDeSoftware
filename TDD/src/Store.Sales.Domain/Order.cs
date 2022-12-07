using FluentValidation.Results;
using Store.Core.DomainObjects;

namespace Store.Sales.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        public static int MAX_UNITS_ITEM => 15;
        public static int MIN_UNITS_ITEM => 1;

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Guid ClientId { get; private set; }

        public decimal Amount { get; private set; }
        public decimal Discount { get; private set; }

        public OrderStatus OrderStatus { get; private set; }

        public bool VoucherUsed { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var result = voucher.ValidateIfApplicable();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUsed = true;

            CalculateTotalDiscountAmount();

            return result;
        }

        public void CalculateTotalDiscountAmount()
        {
            if (!VoucherUsed) return;

            decimal discount = 0;
            var amount = Amount;

            if (Voucher.TypeDiscountVoucher == TypeDiscountVoucher.Value)
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    amount -= discount;
                }
            }
            else
            {
                if (Voucher.PercentageDiscount.HasValue)
                {
                    discount = (Amount * Voucher.PercentageDiscount.Value) / 100;
                    amount -= discount;
                }
            }

            Amount = amount < 0 ? 0 : amount;
            Discount = discount;
        }

        private void CalculateOrderValue()
        {
            Amount = OrderItems.Sum(i => i.CalculateValue());
            CalculateTotalDiscountAmount();
        }

        public bool ExistingItemOrder(OrderItem item)
        {
            return _orderItems.Any(p => p.ProductId == item.ProductId);
        }

        private void ValidateOrderNonexistentItem(OrderItem item)
        {
            if (!ExistingItemOrder(item)) throw new DomainException("O item não pertence ao pedido");
        }

        private void ValidateAllowedItemQuantity(OrderItem item)
        {
            var quantityItems = item.Quantity;
            if (ExistingItemOrder(item))
            {
                var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);
                quantityItems += itemExisting.Quantity;
            }

            if (quantityItems > MAX_UNITS_ITEM) throw new DomainException($"Máximo de {MAX_UNITS_ITEM} unidades por produto.");
        }

        public void AddItem(OrderItem orderItem)
        {
            ValidateAllowedItemQuantity(orderItem);

            if (ExistingItemOrder(orderItem))
            {
                var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

                itemExisting.AddUnits(orderItem.Quantity);
                orderItem = itemExisting;
                _orderItems.Remove(itemExisting);
            }

            _orderItems.Add(orderItem);
            CalculateOrderValue();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            ValidateOrderNonexistentItem(orderItem);
            ValidateAllowedItemQuantity(orderItem);

            var itemExisting = OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

            _orderItems.Remove(itemExisting);
            _orderItems.Add(orderItem);

            CalculateOrderValue();
        }

        public void RemoveItem(OrderItem orderItem)
        {
            ValidateOrderNonexistentItem(orderItem);

            _orderItems.Remove(orderItem);

            CalculateOrderValue();
        }

        public void MakeDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public static class OrderFactory
        {
            public static Order NewOrderDraft(Guid clientId)
            {
                var order = new Order
                {
                    ClientId = clientId,
                };

                order.MakeDraft();
                return order;
            }
        }
    }
}
