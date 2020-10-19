using System;
using System.Collections.Generic;

namespace Hydra.Order.Application.Queries.Dtos
{
    //Aggregate Entity
    public class OrderAggregateDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string VoucherCode { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public OrderPaymentDto Payment { get; set; }
    }
}