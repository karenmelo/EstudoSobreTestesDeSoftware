﻿using MediatR;
using Moq;
using Moq.AutoMock;
using Store.Sales.App.Commands;
using Store.Sales.Domain;

namespace Store.Sales.App.Tests.Order
{
    public class OrderCommandHandlerTests
    {
        [Fact(DisplayName = "Add Item New Order Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task AddItem_NewOrder_MustRunSuccessfully()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(),
                 "Produto Teste", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None));
        }

        [Fact(DisplayName = "Add New Item In Order Draft Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_NewItemOrderDraft_MustRunSuccessfully()
        {
            // Arrange
            var clientId = Guid.NewGuid();

            var order = Store.Sales.Domain.Order.OrderFactory.NewOrderDraft(clientId);
            var orderItemExisting = new OrderItem(Guid.NewGuid(), "Produto XPTO", 2, 100);
            order.AddItem(orderItemExisting);

            var orderCommand = new AddOrderItemCommand(clientId, Guid.NewGuid(), "Produto Teste", 3, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>()
                  .Setup(r => r.GetOrderDraftByClientId(clientId)).Returns(Task.FromResult(order));
            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));


            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);


            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.AddItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Existing Item In Order Draft Successfully")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_ExistingItemOrderDraft_MustRunSuccessfully()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var order = Store.Sales.Domain.Order.OrderFactory.NewOrderDraft(clientId);
            var orderItemExisting = new OrderItem(productId, "Produto XPTO", 3, 100);
            order.AddItem(orderItemExisting);

            //Add new quantity of the existing item
            var orderCommand = new AddOrderItemCommand(clientId, productId, "Produto XPTO", 6, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>()
                 .Setup(r => r.GetOrderDraftByClientId(clientId)).Returns(Task.FromResult(order));
            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Store.Sales.Domain.Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);

        }

        [Fact(DisplayName = "Add Invalid Item Command Should Return False and Throw Notification Events")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async void AddItem_InvalidCommand_ShouldReturnFalseAndThrowNotificationEvents()
        {
            // Arrange           
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
