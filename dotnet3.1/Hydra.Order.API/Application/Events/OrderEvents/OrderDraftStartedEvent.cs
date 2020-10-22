using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.OrderEvents
{
    public class OrderDraftStartedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public string VoucherCode { get; private set; }
        public Guid OrderId { get; private set; }
        public bool VoucherApplied { get; set; }

        public OrderDraftStartedEvent(Guid customerId, string voucherCode, Guid orderId, bool voucherApplied)
        {
            /// All command triggered has reference with Aggregation root where orderId is the aggregation root from Order entity
            CustomerId = customerId;
            VoucherCode = voucherCode;
            OrderId = orderId;
            AggregateId = orderId;
            VoucherApplied = voucherApplied;
        }
    }
}