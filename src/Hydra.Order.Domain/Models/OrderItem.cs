using System;
using Hydra.Core.DomainObjects;

namespace Hydra.Order.Domain.Models
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; }

        public int Qty { get; private set; }

        public decimal Price { get; private set; }

        public OrderItem(Guid productId, string productName, int qty, decimal price)
        {
            if(qty> Order.MAX_QTY_PER_ITEM) throw new DomainException($"Maximum {Order.MAX_QTY_PER_ITEM} per item");
            if(qty < Order.MIN_QTY_PER_ITEM) throw new DomainException($"Minimum {Order.MIN_QTY_PER_ITEM} per item");

            ProductId = productId;
            ProductName = productName;
            Qty = qty;
            Price = price;
        }

        internal void AddQty(int qty) => Qty += qty;

        internal decimal CalculateAmount() => Qty * Price;
    }
}