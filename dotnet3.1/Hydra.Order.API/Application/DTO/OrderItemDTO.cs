using System;
using Hydra.Order.Domain.Orders;

namespace Hydra.Order.API.Application.DTO
{
    public class OrderItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public Guid OrderId { get; set; }

        public static OrderItem Map(OrderItemDTO item)
        {
            return new OrderItem(item.ProductId, item.ProductName, item.Qty,
                item.Price, item.ProductImage);
        }
    }
}