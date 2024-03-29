﻿using Store.Core.Messages;

namespace Store.Sales.App.Events
{
    public class AddOrderItemEvent : Event
    {
        public AddOrderItemEvent(Guid clientId, Guid orderId, Guid productId, string productName, decimal unitValue, int quantity)
        {
            AggregateId = productId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            UnitValue = unitValue;
            Quantity = quantity;
        }

        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal UnitValue { get; private set; }
        public int Quantity { get; private set; }


    }
}
