using System;
using System.Collections.Generic;
using Hydra.Core.Messages;

namespace Hydra.Order.API.Application.Commands.CatalogCommands
{
    public class ProductValidationInStockCommand : Command
    {
        public List<Guid> Products { get; set; }

        public ProductValidationInStockCommand(Guid orderId, List<Guid> products)
        {
            Products = products;
            AggregateId = orderId;
        }
    }
}