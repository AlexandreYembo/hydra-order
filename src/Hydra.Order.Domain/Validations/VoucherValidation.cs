using System;
using FluentValidation;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Models;

namespace Hydra.Order.Domain.Validations
{
    public class VoucherValidation : AbstractValidator<Voucher>
    {
        public static string CodeError => "Invalid code for this voucher";
        public static string VoucherExpiredError => "Voucher expired";
        public static string InactiveVoucherError => "Voucher is not valid";
        public static string UsedVoucherError => "Voucher was used";
        public static string QtyError => "Voucher is not available";
        public static string DiscountValueError => "Amount of discount may  be greater than 0";
        public static string PercentageDiscountError => "Percentage of discount may to be greater than 0";

        public VoucherValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeError);

            RuleFor(c => c.ExpirationDate)
                .Must(ExpirationDateGreaterThanNow)
                .WithMessage(VoucherExpiredError);

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage(InactiveVoucherError);

            RuleFor(c => c.IsUsed)
                .Equal(false)
                .WithMessage(UsedVoucherError);
            
            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QtyError);
            
            When(c => c.VoucherType == VoucherType.Value, () =>{
                RuleFor(f => f.DiscountAmount)
                    .NotNull()
                    .WithMessage(DiscountValueError)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueError);
            });

            When(c => c.VoucherType == VoucherType.Percentage, () =>{
                RuleFor(f => f.DiscountPercentage)
                    .NotNull()
                    .WithMessage(PercentageDiscountError)
                    .GreaterThan(0)
                    .WithMessage(PercentageDiscountError);
            });
        }

        protected static bool ExpirationDateGreaterThanNow(DateTime expiration) => expiration >= DateTime.Now;
    }
}