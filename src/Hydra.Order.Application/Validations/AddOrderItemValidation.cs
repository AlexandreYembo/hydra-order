using System;
using FluentValidation;
using Hydra.Order.Application.Queries;

namespace Hydra.Order.Application.Validations
{
    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string CustomerIdErrorMsg => "Customer Id is invalid";
        public static string ProductIdErrorMsg => "Product Id is invalid";
        public static string ProductNameErrorMsg => "Product name not informed";
        public static string MaxQtyErrorMsg => $"Maximum quantity for this item is {Domain.Models.Order.MAX_QTY_PER_ITEM}";
        public static string MinQtyErrorMsg => $"Minimum quantity for this item is {Domain.Models.Order.MIN_QTY_PER_ITEM}";
        public static string PriceErrorMsg => "Price should be greater than 0";
        public AddOrderItemValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg);

            RuleFor(c => c.ProductName)
                .NotEmpty()
                .WithMessage(ProductNameErrorMsg);

            RuleFor(c => c.Qty)
                .GreaterThan(Domain.Models.Order.MIN_QTY_PER_ITEM)
                .WithMessage(MinQtyErrorMsg)
                .LessThanOrEqualTo(Domain.Models.Order.MAX_QTY_PER_ITEM)
                .WithMessage(MaxQtyErrorMsg);
            
            RuleFor(c => c.Price)
                .GreaterThan(0)
                .WithMessage(PriceErrorMsg);

        }
    }
}