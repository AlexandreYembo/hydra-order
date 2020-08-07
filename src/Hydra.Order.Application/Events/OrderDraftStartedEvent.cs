using System;
using Hydra.Core.Messages;

namespace Hydra.Order.Application.Events
{
    public class OrderDraftStartedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }

        public OrderDraftStartedEvent(Guid customerId, Guid orderId)
        {
            /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            AggregateId = orderId;
            
            CustomerId = customerId;
            OrderId = orderId;
        }
    }
}