using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hydra.Order.Application.Queries.Dtos;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Repository;

namespace Hydra.Order.Application.Queries
{
    /// <summary>
    /// Query facade for Order
    /// </summary>
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderAggregateDto> GetAggregatedOrderCustomer(Guid customerId)
        {
            var order = await _orderRepository.GetOrderDraftByCustomerId(customerId);
            if(order == null) return null;

            var orderAggregate = new OrderAggregateDto
            {
                CustomerId = order.CustomerId,
                Amount = order.Amount,
                OrderId = order.Id,
                Discount = order.DiscountApplied,
                Price = order.DiscountApplied + order.Amount
            };

            if(order.VourcherId != null)
            orderAggregate.VoucherCode = order.Voucher.Code;

            foreach (var item in order.OrderItems)
            {
                orderAggregate.Items.Add(new OrderItemDto
                {
                    ProductId = item.OrderId,
                    ProductName = item.ProductName,
                    Qty = item.Qty,
                    Price = item.Price,
                    Amount = item.Price * item.Qty
                });
            }

            return orderAggregate;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersCustomer(Guid customerId)
        {
            var orders = await _orderRepository.GetListOfOrderByClientId(customerId);
            orders = orders.Where(w => w.OrderStatus == OrderStatus.Processed || w.OrderStatus == OrderStatus.Cancelled)
                            .OrderByDescending(o => o.Code);

            if(!orders.Any()) return null;

            var ordersDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                ordersDto.Add(new OrderDto
                {
                    Amount = order.Amount,
                    OrderStatus = (int) order.OrderStatus,
                    Code = order.Code,
                    CreatedDate = order.CreatedDate
                });
            }
            
            return ordersDto;
        }
    }
}