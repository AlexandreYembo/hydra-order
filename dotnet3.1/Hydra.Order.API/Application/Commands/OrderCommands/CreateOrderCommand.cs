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
        //Order
        public CreateOrderCommand(Guid customerId, decimal totalPrice,
        List<OrderItemDTO> items, 
        string voucherCode, bool hasVoucher, decimal discount, AddressDTO address, 
        string cardNumber, string cardHolderName, string cardExpiration, string cardCvv)
        {
            this.CustomerId = customerId;
            this.TotalPrice = totalPrice;
            this.VoucherCode = voucherCode;
            this.HasVoucher = hasVoucher;
            this.Discount = discount;
            this.Address = address;
            this.CardNumber = cardNumber;
            this.CardHolderName = cardHolderName;
            this.CardExpiration = cardExpiration;
            this.CardCvv = cardCvv;
            this.Items = items;

        }
        public Guid CustomerId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDTO> Items { get; set; }

        //Voucher
        public string VoucherCode { get; set; }
        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }

        //Address
        public AddressDTO Address { get; set; }

        //Payment - CARD
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiration { get; set; }
        public string CardCvv { get; set; }

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

            RuleFor(c => c.CardNumber)
                .CreditCard()
                .WithMessage("Invalid Card number");

            RuleFor(c => c.CardHolderName)
                .NotNull()
                .WithMessage("Card holder name is required");

            RuleFor(c => c.CardCvv.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("CVV should have 3 or 4 numbers");

            RuleFor(c => c.CardExpiration)
                .NotNull()
                .WithMessage("Expiration date of the card is required");
        }
    }
}