using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.OrderCommands
{
    public class ProcessingOrderCommand : Command
    {
        public ProcessingOrderCommand(Guid orderId)
        {
            AggregateId = orderId;
        }
    }
}