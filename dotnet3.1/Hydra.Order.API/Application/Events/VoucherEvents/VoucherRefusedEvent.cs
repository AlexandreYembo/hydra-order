using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.VoucherEvents
{
    public class VoucherRefusedEvent : Event
    {
        public VoucherRefusedEvent(Guid orderId, Guid customerId, string voucherCode)
        {
            this.OrderId = orderId;
            this.CustomerId = customerId;
            this.AggregateId = orderId;
            this.VoucherCode = voucherCode;

        }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string VoucherCode { get; set; }
    }
}