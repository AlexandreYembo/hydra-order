using System;
using Hydra.Core.Messages;
using Hydra.Order.Application.Validations;

namespace Hydra.Order.Application.Commands
{
    public class UpdateOrderItemCommand : Command
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public int Qty { get; set; }

        public UpdateOrderItemCommand(Guid customerId, Guid productId, int qty)
        {
            CustomerId = customerId;
            ProductId = productId;
            Qty = qty;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}