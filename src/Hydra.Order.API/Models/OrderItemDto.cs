using System;

namespace Hydra.Order.API.Models
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public int Qty { get; set; }
    }
}