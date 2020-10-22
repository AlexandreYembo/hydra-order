using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.VoucherEvents
{
    public class VoucherAppliedEvent : Event
    {
        public VoucherAppliedEvent(Guid orderId, Guid customerId, Guid voucherId)
        {
            this.OrderId = orderId;
            this.CustomerId = customerId;
            this.AggregateId = orderId;
            this.VoucherId = voucherId;
        }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid VoucherId { get; set; }
    }
}