using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Order.Application.Queries.Dtos;

namespace Hydra.Order.Application.Queries
{
    /// <summary>
    /// From Readonly database
    /// </summary>
    public interface IOrderQueries
    {
        Task<OrderAggregateDto> GetAggregatedOrderCustomer(Guid customerId);
        Task<IEnumerable<OrderDto>> GetOrdersCustomer(Guid customerId);
    }
}