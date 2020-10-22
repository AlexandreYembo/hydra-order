using System;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Events.CatalogEvents
{
    public class ProductInStockCheckedEvent : Event
    {
        public Guid OrderId { get; set; }

        public ProductInStockCheckedEvent(Guid orderId)
        {
            AggregateId = orderId;
        }
    }
}