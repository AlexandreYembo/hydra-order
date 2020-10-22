using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.VoucherEvents
{
    public class UpdateVoucherUsedFailedEvent : Event
    {
        public UpdateVoucherUsedFailedEvent(Guid orderId, Guid voucherId)
        {
            this.OrderId = orderId;
            this.AggregateId = orderId;
            this.VoucherId = voucherId;
        }
        public Guid OrderId { get; set; }
        public Guid VoucherId { get; set; }
    }
}