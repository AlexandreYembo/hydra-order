using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.VoucherCommands
{
    public class ApplyVoucherOrderCommand : Command
    {
        public ApplyVoucherOrderCommand(string code, Guid customerId, Guid orderId, bool voucherApplied)
        {
            this.OrderId = orderId;
            AggregateId = orderId;
            this.Code = code;
            this.CustomerId = customerId;
            this.VoucherApplied = voucherApplied;
        }
        public string Code { get; set; }
        public Guid CustomerId { get; set; }
        public Guid OrderId { get; set; }
        public bool VoucherApplied { get; set; }
    }
}