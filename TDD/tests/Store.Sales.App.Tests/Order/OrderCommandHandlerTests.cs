using MediatR;
using Moq;
using Moq.AutoMock;
using Store.Sales.App.Commands;
using Store.Sales.Domain;

namespace Store.Sales.App.Tests.Order
{
    public class OrderCommandHandlerTests
    {
        private readonly Guid _clientId;
        private readonly Guid _productId;
        private readonly Domain.Order _order;
        private readonly AutoMocker _mocker;
        private readonly OrderCommandHandler _orderHandler;

        public OrderCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _orderHandler = _mocker.CreateInstance<OrderCommandHandler>();
            _clientId = Guid.NewGuid();
            _productId = Guid.NewGuid();
            _order = Domain.Order.OrderFactory.NewOrderDraft(_clientId);
        }

        [Fact(DisplayName = "Add Item New Order Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddItem_NewOrder_MustRunSuccessfully()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(_clientId, _productId,
                 "Produto Teste", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "Add New Item In Order Draft Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_NewItemOrderDraft_MustRunSuccessfully()
        {
            // Arrange
            var order = Domain.Order.OrderFactory.NewOrderDraft(_clientId);
            var orderItemExisting = new OrderItem(_productId, "Produto XPTO", 2, 100);
            order.AddItem(orderItemExisting);

            var orderCommand = new AddOrderItemCommand(_clientId, Guid.NewGuid(), "Produto Teste", 3, 100);

            _mocker.GetMock<IOrderRepository>()
                  .Setup(r => r.GetOrderDraftByClientId(_clientId)).Returns(Task.FromResult(order));
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.AddItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Existing Item In Order Draft Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_ExistingItemOrderDraft_MustRunSuccessfully()
        {
            // Arrange 
            var orderItemExisting = new OrderItem(_productId, "Produto XPTO", 3, 100);
            _order.AddItem(orderItemExisting);

            //Add new quantity of the existing item
            var orderCommand = new AddOrderItemCommand(_clientId, _productId, "Produto XPTO", 6, 100);

            _mocker.GetMock<IOrderRepository>()
                 .Setup(r => r.GetOrderDraftByClientId(_clientId)).Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);

        }

        [Fact(DisplayName = "Add Invalid Item Command Should Return False and Throw Notification Events")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_InvalidCommand_ShouldReturnFalseAndThrowNotificationEvents()
        {
            // Arrange           
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
