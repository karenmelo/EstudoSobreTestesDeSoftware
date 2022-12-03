using Store.Core.DomainObjects;

namespace Store.Sales.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "New Item Below Allowed")]
        [Trait("Category", "Sales - Order Item")]
        public void AddOrderItem_ItemUnitsBelowAllowed_MustReturnException()
        {
            // Arrange && Act  && Assert         
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Product Test", Order.MIN_UNITS_ITEM - 1, 100));
        }
    }
}
