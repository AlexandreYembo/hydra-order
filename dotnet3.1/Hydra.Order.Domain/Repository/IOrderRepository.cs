using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Core.Data;

namespace Hydra.Order.Domain.Repository
{
    public interface IOrderRepository : IRepository<Models.Order>
    {
        Task<Models.Order> GetOrderById(Guid id);
        Task<IEnumerable<Models.Order>> GetListOfOrderByClientId(Guid customerId);
        Task<Models.Order> GetOrderDraftByCustomerId(Guid customerId);
        void AddOrder(Models.Order order);
        void UpdateOrder(Models.Order order);

        Task<Models.OrderItem> GetOrderItemById(Guid id);

        Task<Models.OrderItem> GetOrderItemByOrder(Guid orderId, Guid productId);

        void AddOrderItem(Models.OrderItem orderItem);

        void UpdateOrderItem(Models.OrderItem orderItem);

        void RemoveItem(Models.OrderItem orderItem);
    }
}