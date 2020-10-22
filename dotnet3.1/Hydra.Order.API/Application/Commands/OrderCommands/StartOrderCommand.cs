using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.OrderCommands
{
    public class StartOrderCommand : Command
    {
        public StartOrderCommand(Guid orderId, Guid customerId)
        {
            this.OrderId = orderId;
            this.CustomerId = customerId;

        }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}