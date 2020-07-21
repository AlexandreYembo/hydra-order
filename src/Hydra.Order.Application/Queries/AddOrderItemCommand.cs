using System;
using FluentValidation.Results;
using Hydra.Core.Messages;
using Hydra.Order.Application.Validations;

namespace Hydra.Order.Application.Queries
{
    public class AddOrderItemCommand : Command
    {
        public Guid CustomerId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Qty { get; private set; }
        public decimal Price { get; private set; }
        public AddOrderItemCommand(Guid customerId, Guid productId, string productName, int qty, decimal price)
        {
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            Qty = qty;
            Price = price;  
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}