using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Hydra.Core.Messages;
using Hydra.Order.API.Application.DTO;

namespace Hydra.Order.API.Application.Commands.OrderCommands
{
    public class CreateOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> Items { get; set; }

        //Voucher
        public VoucherDTO Voucher { get; set; }
        public bool HasVoucher { get; set; }
      
        //Address
        public AddressDTO Address { get; set; }

        public PaymentDTO Payment {get; set;}

        //Payment - CARD
        public override bool IsValid()
        {
            ValidationResult = new CreateOrderValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateOrderValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidation()
        {
            RuleFor(c => c.CustomerId)
                .NotEqual(Guid.Empty)
                .WithMessage("Customer Id invalid");

            RuleFor(c => c.Items.Count())
                .GreaterThan(0)
                .WithMessage("Order may have at least 1 item");

            RuleFor(c => c.TotalPrice)
                .GreaterThan(0)
                .WithMessage("Invalid Price");

            RuleFor(c => c.Payment.CardNumber)
                .NotNull()
                .WithMessage("Card number is required")
                .CreditCard()
                .WithMessage("Invalid Card number");

            RuleFor(c => c.Payment.CardHolderName)
                .NotNull()
                .WithMessage("Card holder name is required");

            RuleFor(c => c.Payment.CardCvv)
                .NotNull()
                .WithMessage("Cvv is required");

            RuleFor(c => c.Payment.CardCvv.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("CVV should have 3 or 4 numbers");

            RuleFor(c => c.Payment.CardExpiration)
                .NotNull()
                .WithMessage("Expiration date of the card is required");
        }
    }
}