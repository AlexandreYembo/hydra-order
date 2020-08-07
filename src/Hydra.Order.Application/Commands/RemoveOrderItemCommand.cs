using System;
using Hydra.Core.Messages;
using Hydra.Order.Application.Validations;

namespace Hydra.Order.Application.Commands
{
    public class RemoveOrderItemCommand : Command
    {
         public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }

        public RemoveOrderItemCommand(Guid customerId, Guid orderId, Guid productId)
        {
            CustomerId = customerId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        } 
    }
}