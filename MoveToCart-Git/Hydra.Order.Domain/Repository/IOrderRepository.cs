using System;
using System.Threading.Tasks;
using Hydra.Core.Data;

namespace Hydra.Order.Domain.Repository
{
    public interface IOrderRepository : IRepository<Models.Order>
    {
         void AddOrder(Models.Order order);
         void UpdateOrder(Models.Order order);

         void AddOrderItem(Models.OrderItem orderItem);

         void UpdateOrderItem(Models.OrderItem orderItem);
         Task<Models.Order> GetOrderDraftByCustomerId(Guid customerId);

    }
}