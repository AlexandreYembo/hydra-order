using System;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Validations;

namespace Hydra.Order.Domain.Models
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; }

        public int Qty { get; private set; }

        public decimal Price { get; private set; }

        public Order Order { get; set; }

        public OrderItem(Guid productId, string productName, int qty, decimal price)
        {
            if(qty < Order.MIN_QTY_PER_ITEM) throw new DomainException($"Minimum {Order.MIN_QTY_PER_ITEM} per item");

            ProductId = productId;
            ProductName = productName;
            Qty = qty;
            Price = price;
        }

        internal void AddOrder(Guid orderId) => OrderId = orderId;
        internal void AddQty(int qty) => Qty += qty;
        internal decimal CalculateAmount() => Qty * Price;
        internal void UpdateQty(int qty) => Qty = qty;

        public ValidationResult IsValid()
        {
            return new OrderItemValidation().Validate(this);
        }
    }
}