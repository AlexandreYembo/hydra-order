using FluentValidation;
using Hydra.Order.Domain.Orders;

namespace Hydra.Order.Domain.Validations
{
    public class OrderItemValidation : AbstractValidator<OrderItem>
    {
        public OrderItemValidation()
        {
        }
    }
}