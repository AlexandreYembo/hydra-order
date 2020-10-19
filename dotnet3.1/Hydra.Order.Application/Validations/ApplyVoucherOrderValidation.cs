using System;
using FluentValidation;
using Hydra.Order.Application.Commands;

namespace Hydra.Order.Application.Validations
{
    public class ApplyVoucherOrderValidation: AbstractValidator<ApplyVoucherOrderCommand>
    {
        public static string CustomerIdErrorMsg => "Customer Id is invalid";
        public static string VoucherCodeErrorMsg => "Voucher code can't be empty";
        public static string OrderIdErrorMsg => "Order Id is invalid";
        public ApplyVoucherOrderValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage(CustomerIdErrorMsg);
                
            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage(VoucherCodeErrorMsg);

           
        }
    }
}