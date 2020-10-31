using System;
using System.Collections.Generic;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.OrderEvents
{
    public class OrderStartedEvent : Event
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }

        public OrderStartedEvent(Guid orderId, Guid customerId)
        {
            OrderId = orderId;
            CustomerId = customerId;
        }
    }
}