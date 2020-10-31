using System;
using System.Collections.Generic;

namespace Hydra.Order.API.Application.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }

        public decimal Discount { get; set; }

        public bool HasVoucher { get; set; }

        public List<OrderItemDTO> Items { get; set; }

        public AddressDTO Address { get; set; }

        public static OrderDTO Map(Domain.Orders.Order order)
        {
            var orderDTO = new OrderDTO{
                Id = order.Id,
                Code = order.Code,
                Status = (int)order.OrderStatus,
                CreatedDate = order.CreatedDate,
                Amount = order.Amount,
                Discount = order.DiscountApplied,
                HasVoucher = order.IsUsedVoucher,
                Items = new List<OrderItemDTO>(),
                Address = new AddressDTO()
            };

            foreach (var item in order.OrderItems)
            {
                orderDTO.Items.Add(new OrderItemDTO{
                    Name = item.ProductName,
                    Image = item.Image,
                    Qty = item.Qty,
                    ProductId = item.ProductId,
                    Price = item.Price,
                    OrderId = item.OrderId
                });
            }

            return orderDTO;
        }
    }
}