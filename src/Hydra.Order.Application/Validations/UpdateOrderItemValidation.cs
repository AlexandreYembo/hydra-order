using System;
using FluentValidation;
using Hydra.Order.Application.Commands;

namespace Hydra.Order.Application.Validations
{
    public class UpdateOrderItemValidation: AbstractValidator<UpdateOrderItemCommand>
    {
        public static string CustomerIdErrorMsg => "Customer Id is invalid";
        public static string ProductIdErrorMsg => "Product Id is invalid";
        public static string OrderIdErrorMsg => "Order Id is invalid";
        public static string MaxQtyErrorMsg => $"Maximum quantity for this item is {Domain.Models.Order.MAX_QTY_PER_ITEM}";
        public static string MinQtyErrorMsg => $"Minimum quantity for this item is {Domain.Models.Order.MIN_QTY_PER_ITEM}";
        public UpdateOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg);

            RuleFor(c => c.Qty)
                .GreaterThan(Domain.Models.Order.MIN_QTY_PER_ITEM)
                .WithMessage(MinQtyErrorMsg)
                .LessThanOrEqualTo(Domain.Models.Order.MAX_QTY_PER_ITEM)
                .WithMessage(MaxQtyErrorMsg);

        }
    }
}