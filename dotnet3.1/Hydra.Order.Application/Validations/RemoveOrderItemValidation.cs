using System;
using FluentValidation;
using Hydra.Order.Application.Commands;

namespace Hydra.Order.Application.Validations
{
    public class RemoveOrderItemValidation: AbstractValidator<RemoveOrderItemCommand>
    {
        public static string CustomerIdErrorMsg => "Customer Id is invalid";
        public static string ProductIdErrorMsg => "Product Id is invalid";
        public static string OrderIdErrorMsg => "Order Id is invalid";
      
        public RemoveOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg);
        }
    }
}