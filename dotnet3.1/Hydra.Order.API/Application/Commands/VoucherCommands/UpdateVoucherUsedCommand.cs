using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.VoucherCommands
{
    public class UpdateVoucherUsedCommand : Command
    {
        public UpdateVoucherUsedCommand(Guid voucherId, Guid orderId)
        {
            this.VoucherId = voucherId;
            this.OrderId = orderId;
            AggregateId = orderId;

        }
        public Guid VoucherId { get; set; }
        public Guid OrderId { get; set; }
    }
}