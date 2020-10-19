using System;
using Hydra.Core.Messages;

namespace Hydra.Order.Application.Events
{
    public class OrderUpdatedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public decimal Amount { get; private set; }

        public OrderUpdatedEvent(Guid customerId, Guid orderId, decimal amount)
        {
            /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            AggregateId = orderId;
            
            CustomerId = customerId;
            OrderId = orderId;
            Amount = amount;
        }
    }
}