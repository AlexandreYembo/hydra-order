using System;
using System.Collections.Generic;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.OrderEvents
{
    public class OrderStartedEvent : Event
    {
        public List<Guid> Products { get; set; }

        public OrderStartedEvent(List<Guid> products)
        {
            Products = products;
        }
    }
}