using Store.Core.DomainObjects;

namespace Store.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add New Order Item ")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_NewOrder_MustUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(200, order.Amount);
        }

        [Fact(DisplayName = "Add Existing Order Item ")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ExistingItem_MustIncrementUnitsAddValues()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Product Test", 1, 100);

            // Act
            order.AddItem(orderItem2);

            // Assert
            Assert.Equal(300, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems?.FirstOrDefault(o => o.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Add Item Order Above Allowed")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ItemUnitsAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Produtct Test", Order.MAX_UNITS_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Existing Item Adds Units Above Allowed")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ExistingItemAddsUnitsAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Produtct Test", 1, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Produtct Test", Order.MAX_UNITS_ITEM, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }


        [Fact(DisplayName = "Update Order Item Nonexistent")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_ItemDoesNotExistList_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var updatedOrderItem = new OrderItem(Guid.NewGuid(), "Product Test", 5, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }
    }
}
