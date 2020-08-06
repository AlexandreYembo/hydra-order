using System;
using FluentValidation;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Models;

namespace Hydra.Order.Domain.Validations
{
    public class OrderItemValidation : AbstractValidator<OrderItem>
    {
        public OrderItemValidation()
        {
        }
    }
}