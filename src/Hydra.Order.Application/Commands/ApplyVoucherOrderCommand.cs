using System;
using Hydra.Core.Messages;
using Hydra.Order.Application.Validations;

namespace Hydra.Order.Application.Commands
{
    public class ApplyVoucherOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public string VoucherCode { get; set; }

        public ApplyVoucherOrderCommand(Guid customerId, string voucherCode)
        {
            CustomerId = customerId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyVoucherOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}