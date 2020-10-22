using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.VoucherEvents
{
    public class OrderWithoutVoucherVerifiedEvent : Event
    {
        public OrderWithoutVoucherVerifiedEvent(Guid orderId, Guid customerId)
        {
            this.OrderId = orderId;
            this.CustomerId = customerId;
            AggregateId = orderId;
        }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}