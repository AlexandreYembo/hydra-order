using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hydra.Core.Data;

namespace Hydra.Order.Domain.Repository
{
    public interface IOrderRepository : IRepository<Orders.Order>
    {
        Task<Orders.Order> GetOrderById(Guid id);
        Task<IEnumerable<Orders.Order>> GetListOfOrderByCustomerId(Guid customerId);
        Task<Orders.Order> GetOrderDraftByCustomerId(Guid customerId);
        void AddOrder(Orders.Order order);
        void UpdateOrder(Orders.Order order);

        Task<Orders.OrderItem> GetOrderItemById(Guid id);

        Task<Orders.OrderItem> GetOrderItemByOrder(Guid orderId, Guid productId);

        void AddOrderItem(Orders.OrderItem orderItem);

        void UpdateOrderItem(Orders.OrderItem orderItem);

        void RemoveItem(Orders.OrderItem orderItem);
    }
}