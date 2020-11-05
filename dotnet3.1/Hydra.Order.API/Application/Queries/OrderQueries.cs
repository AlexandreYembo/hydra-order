using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Hydra.Order.API.Application.DTO;
using Hydra.Order.Domain.Repository;

namespace Hydra.Order.API.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<OrderDTO> GetAuthorizedOrders()
        {
             //Temporary, using dapper
            const string sql = @"SELECT
                                O.ID as 'OrderId', O.ID, O.CustomerId,
                                OI.ID AS 'OrderItemId', OI.ID, OI.ProductId, OI.Qty
                                FROM ORDERS O 
                                INNER JOIN ORDERITEMS OI ON O.ID = OI.ORDERID 
                                WHERE O.ORDERSTATUS = 1 
                                ORDER BY O.CREATEDDATE";
            
            var lookup = new Dictionary<Guid, OrderDTO>();
            var order = await _orderRepository.GetConnection().QueryAsync<OrderDTO, OrderItemDTO, OrderDTO>(sql,
            (o, oi) => 
            {
                if (!lookup.TryGetValue(o.Id, out var orderDTO))
                        lookup.Add(o.Id, orderDTO = o);

                orderDTO.Items ??= new List<OrderItemDTO>();
                orderDTO.Items.Add(oi);

                return o;
            }, splitOn: "OrderId, OrderItemId" );

            return lookup.Values.OrderBy(p=>p.CreatedDate).FirstOrDefault();
        }

        public async Task<OrderDTO> GetLatestOrder(Guid customerId)
        {
            //Temporary, using dapper
            const string sql = @"SELECT
                                O.Id as 'OrderId', O.Code, O.IsUsedVoucher, O.DiscountApplied, O.Amount, O.DiscountApplied, O.OrderStatus, 
                                O.StreetName, O.Number, O.City, O.State, O.PostCode, O.Country,
                                OI.ID AS 'OrderItemId', OI.ProductName, OI.Qty, OI.Image, OI.Price
                                FROM ORDERS O 
                                INNER JOIN ORDERITEMS OI ON O.ID = OI.ORDERID 
                                WHERE O.CUSTOMERID = @customerId 
                                AND O.CREATEDDATE between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND O.ORDERSTATUS = 1 
                                ORDER BY O.CREATEDDATE DESC";
            
            var order = await _orderRepository.GetConnection()
                            .QueryAsync(sql, new { customerId });

            return MapOrder(order);
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByCustomerId(Guid customerId)
        {
            var orders = await _orderRepository.GetListOfOrderByCustomerId(customerId);
            return orders.Select(OrderDTO.Map);
        }

        private OrderDTO MapOrder(dynamic result)
        {
            var order = new OrderDTO(){
                Code = result[0].Code,
                Status = result[0].OrderStatus,
                Amount = result[0].Amount,
                Discount = result[0].DiscountApplied,
                HasVoucher = result[0].IsUsedVoucher,

                Items = new List<OrderItemDTO>(),
                Address = new AddressDTO
                {
                    Street = result[0].StreetName,
                    Number = result[0].Number,
                    City = result[0].City,
                    State = result[0].State,
                    Country = result[0].Country,
                    PostCode = result[0].PostCode
                }
            };

            foreach (var item in result)
            {
                var orderItem = new OrderItemDTO
                {
                    Name = item.ProductName,
                    Price = item.Price,
                    Qty = item.Qty,
                    Image = item.Image
                };
                order.Items.Add(orderItem);
            }

            return order;
        }
    }
}