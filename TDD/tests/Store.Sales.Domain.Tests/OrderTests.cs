namespace Store.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add New Order Item ")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_NewOrder_MustUpdateValue()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(200, order.Amount);
        }

        [Fact(DisplayName = "Add Existing Order Item ")]
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_ExistingItem_MustIncrementUnitsAddValues()
        {
            // Arrange
            var order = new Order();
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Product Test", 1, 100);

            // Act
            order.AddItem(orderItem2);

            // Assert
            Assert.Equal(300, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(o => o.ProductId == productId).Quantity);
        }
    }
}
