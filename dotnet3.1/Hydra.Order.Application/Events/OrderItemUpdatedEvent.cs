using System;
using Hydra.Core.Messages;

namespace Hydra.Order.Application.Events
{
    public class OrderItemUpdatedEvent: Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Qty { get; private set; }
        public OrderItemUpdatedEvent(Guid customerId, Guid orderId, Guid productId, int qty)
        {
             /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            AggregateId = orderId;

            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            Qty = qty;
        }
        
    }
}