using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Validations;

namespace Hydra.Order.Domain.Models
{
    public class Voucher : Entity
    {
        public Voucher(string code, decimal? discountPercentage, decimal? discountAmount, int quantity,
            VoucherType voucherType,  DateTime expirationDate, bool active, bool isUsed)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            DiscountAmount = discountAmount;
            Quantity = quantity;
            VoucherType = voucherType;
            ExpirationDate = expirationDate;
            Active = active;
            IsUsed = isUsed;
        }

        public string Code { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public decimal? DiscountAmount { get; private set; }
        public VoucherType VoucherType { get; set; }
        public int Quantity { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public DateTime UsedDate { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool IsUsed { get; private set; }

        public ICollection<Order> Order { get; set; }

        internal ValidationResult VoucherIsApplicable()
        {
            return new VoucherValidation().Validate(this);
        }
    }
}