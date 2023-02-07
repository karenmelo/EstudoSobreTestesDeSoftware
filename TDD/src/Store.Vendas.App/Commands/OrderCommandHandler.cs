using MediatR;
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
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Quantity, message.UnitValue);
            var order = Order.OrderFactory.NewOrderDraft(message.ClientId);
            order.AddItem(orderItem);

            _orderRepository.Add(order);

            order.AddEvent(new AddOrderItemEvent(order.ClientId,
                order.Id, message.ProductId, message.Name, message.UnitValue, message.Quantity));

            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
