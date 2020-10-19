using System;
using Hydra.Core.Messages;
using MediatR;

namespace Hydra.Order.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Qty { get; private set; }
        public decimal Price { get; private set; }
        public OrderItemAddedEvent(Guid customerId, Guid orderId, Guid productId, string productName, int qty, decimal price)
        {
             /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            AggregateId = orderId;

            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Qty = qty;
            Price = price;  
        }
    }
}