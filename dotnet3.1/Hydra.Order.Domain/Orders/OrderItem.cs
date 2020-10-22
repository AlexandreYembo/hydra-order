using System;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Validations;

namespace Hydra.Order.Domain.Orders
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public string ProductName { get; private set; }

        public int Qty { get; private set; }

        public decimal Price { get; private set; }
        public string Image { get; private set; }

        //EF relationship
        public Order Order { get; set; }

        //EF Constructor
        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, int qty, decimal price, string image)
        {
            if(qty < Order.MIN_QTY_PER_ITEM) throw new DomainException($"Minimum {Order.MIN_QTY_PER_ITEM} per item");

            ProductId = productId;
            ProductName = productName;
            Qty = qty;
            Price = price;
            Image = image;
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