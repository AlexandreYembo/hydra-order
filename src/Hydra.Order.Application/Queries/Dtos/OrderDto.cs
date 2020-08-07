using System;

namespace Hydra.Order.Application.Queries.Dtos
{
    public class OrderDto
    {
        public int Code { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int OrderStatus { get; set; }
    }
}