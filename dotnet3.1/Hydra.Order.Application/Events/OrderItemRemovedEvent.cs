using System;
using Hydra.Core.Messages;

namespace Hydra.Order.Application.Events
{
    public class OrderItemRemovedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public OrderItemRemovedEvent(Guid customerId, Guid orderId, Guid productId)
        {
             /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }
    }
}