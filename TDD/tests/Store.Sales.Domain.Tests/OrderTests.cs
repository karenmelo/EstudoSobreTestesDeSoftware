﻿using Store.Core.DomainObjects;

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

        [Fact(DisplayName = "Update Order Item Existent")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_ValidItem_MustQuantityUpdated()
        {
            //Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 4, 100);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(productId, "Product Test", 3, 100);
            var newQuantity = orderItemUpdated.Quantity;

            // Act
            order.UpdateItem(orderItemUpdated);

            //Assert 
            Assert.Equal(newQuantity, order.OrderItems?.FirstOrDefault(p => p.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Update Ordered Item Validate Total")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_OrderWithDifferentItems_MustUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 4, 100);
            order.AddItem(orderItem);
            var orderItem2 = new OrderItem(productId, "Product xpto", 2, 55);
            order.AddItem(orderItem2);

            var orderItemUpdated = new OrderItem(productId, "Product xpto", 6, 55);
            var totalOrder = orderItem.Quantity * orderItem.UnitValue + orderItemUpdated.Quantity * orderItemUpdated.UnitValue;

            // Act
            order.UpdateItem(orderItemUpdated);

            // Assert
            Assert.Equal(totalOrder, order.Amount);

        }

        [Fact(DisplayName = "Update Ordered Item Units Above Allowed")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_UnitsItemAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItemExistent1 = new OrderItem(productId, "Product Test", 3, 15);
            order.AddItem(orderItemExistent1);

            var orderItemUpdated = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM + 1, 15);

            //Act && Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(orderItemUpdated));
        }

        [Fact(DisplayName = "Remove Order Item Nonexistent")]
        [Trait("Category", "Sales - Order")]
        public void RemoveOrderItem_ItemDoesNotExistList_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var removeOrderItem = new OrderItem(Guid.NewGuid(), "Product Test", 5, 50);

            // Act && Assert
            Assert.Throws<DomainException>(() => order.RemoveItem(removeOrderItem));
        }

        [Fact(DisplayName = "Remove Existing Order Item Should Calculated Total Value")]
        [Trait("Category", "Sales - Order")]
        public void RemoveOrderItem_ExistingItem_MustUpdateTotalValue()
        {
            //Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Product xpto", 2, 100);
            var orderItem2 = new OrderItem(productId, "Product Test", 3, 45);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var totalOrder = orderItem2.Quantity * orderItem2.UnitValue;

            //Act
            order.RemoveItem(orderItem1);

            //Assert
            Assert.Equal(totalOrder, order.Amount);
        }
    }
}
