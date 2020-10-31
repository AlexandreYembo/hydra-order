using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Order.API.Application.DTO;

namespace Hydra.Order.API.Application.Queries
{
    public interface IOrderQueries
    {
        Task<OrderDTO> GetLatestOrder(Guid customerId);
        Task<IEnumerable<OrderDTO>> GetOrdersByCustomerId(Guid customerId);
    }
}