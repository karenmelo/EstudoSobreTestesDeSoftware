using MediatR;
using Store.Core.DomainObjects;
using Store.Core.Messages;
using Store.Sales.App.Events;
using Store.Sales.Domain;

namespace Store.Sales.App.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!await MessageIsValid(message, cancellationToken)) return false;

            var order = await _orderRepository.GetOrderDraftByClientId(message.ClientId);
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitValue);

            if (order == null)
            {
                order = Order.OrderFactory.NewOrderDraft(message.ClientId);
                order.AddItem(orderItem);
                _orderRepository.Add(order);
            }
            else
            {
                var orderItemExisting = order.ExistingItemOrder(orderItem);
                order.AddItem(orderItem);

                if (orderItemExisting)
                    _orderRepository.UpdateItem(orderItem);
                else
                    _orderRepository.AddItem(orderItem);


                _orderRepository.Update(order);
            }

            order.AddEvent(new AddOrderItemEvent(order.ClientId,
                order.Id, message.ProductId, message.Name, message.UnitValue, message.Quantity));

            return await _orderRepository.UnitOfWork.Commit();
        }

        private async Task<bool> MessageIsValid(Command message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                foreach (var error in message.ValidationResult.Errors)
                {
                    await _mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage), cancellationToken);
                }
                return false;
            }

            return true;
        }
    }
}
