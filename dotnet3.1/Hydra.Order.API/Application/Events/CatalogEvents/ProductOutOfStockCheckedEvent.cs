using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.CatalogEvents
{
    public class ProductOutOfStockCheckedEvent : Event
    {
        public ProductOutOfStockCheckedEvent(Guid orderId)
        {
            AggregateId = orderId;
        }
    }
}