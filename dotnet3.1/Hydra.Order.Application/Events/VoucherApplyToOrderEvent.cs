using System;
using Hydra.Core.Messages;

namespace Hydra.Order.Application.Events
{
    public class VoucherApplyToOrderEvent : Event
    {
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public Guid VoucherId { get; set; }

        public VoucherApplyToOrderEvent(Guid customerId, Guid orderId, Guid voucherId)
        {
            CustomerId = customerId;
            OrderId = orderId;
            VoucherId = voucherId;
        }
    }
}