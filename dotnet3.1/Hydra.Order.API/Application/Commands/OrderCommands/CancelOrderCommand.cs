using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.OrderCommands
{
    public class CancelOrderCommand : Command
    {
        public CancelOrderCommand(Guid orderId)
        {
            this.OrderId = orderId;
            AggregateId = orderId;

        }
        public Guid OrderId { get; set; }
    }
}