using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Core.Data;
using Hydra.Order.Data;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Orders;
using Hydra.Order.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hydra.Catalog.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context){
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public void AddOrder(Order.Domain.Orders.Order order)
        {
            _context.Order.Add(order);
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItem.Add(orderItem);
        }

        public async Task<IEnumerable<Order.Domain.Orders.Order>> GetListOfOrderByCustomerId(Guid customerId)
        {
            return await _context.Order.AsNoTracking().Where(w => w.CustomerId == customerId).ToListAsync();
        }

        public async Task<Order.Domain.Orders.Order> GetOrderById(Guid id)
        {
            return await _context.Order.AsNoTracking()
                                       .Include(i => i.OrderItems)
                                       .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Order.Domain.Orders.Order> GetOrderDraftByCustomerId(Guid customerId)
        {
            var order = await _context.Order.AsNoTracking().FirstOrDefaultAsync(f => f.CustomerId == customerId && f.OrderStatus == OrderStatus.Draft);

            if(order == null) return null;

            await _context.Entry(order)
                        .Collection(c => c.OrderItems).LoadAsync(); //Load order items -> Kind of lazy load
                        //Collection -> Return list
            
            // if(order.VourcherId != null)
            // {
            //     await _context.Entry(order)
            //             .Reference(r => r.Voucher).LoadAsync(); //Load voucher -> Kind of lazy load
            //             // Reference -> Return one item
            // }

            return order;
        }

        public async Task<OrderItem> GetOrderItemById(Guid id)
        {
            return await _context.OrderItem.AsNoTracking().FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<OrderItem> GetOrderItemByOrder(Guid orderId, Guid productId)
        {
            return await _context.OrderItem.AsNoTracking()
                                    .Include(i => i.Order)
                                    .FirstOrDefaultAsync(f => f.OrderId == orderId && f.ProductId == productId);
        }

        public void RemoveItem(OrderItem orderItem)
        {
            _context.OrderItem.Remove(orderItem);
        }

        public void UpdateOrder(Order.Domain.Orders.Order order)
        {
            _context.Order.Update(order);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItem.Update(orderItem);
        }

        public DbConnection GetConnection() => _context.Database.GetDbConnection();

        public void Dispose()
        {
           _context?.Dispose();
        }
        
    }
}
