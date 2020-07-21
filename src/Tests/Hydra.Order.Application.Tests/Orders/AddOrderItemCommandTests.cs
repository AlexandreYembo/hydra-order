using System;
using System.Linq;
using Hydra.Order.Application.Queries;
using Hydra.Order.Application.Validations;
using Xunit;

namespace Hydra.Order.Application.Tests.Orders
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName="Add Order Item Valid Command")]
        [Trait("Order", "Order Comands")]
        public void AddOrderItemCommand_IsValidCommand_ShouldPass()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var orderCommand = new AddOrderItemCommand(customerId, productId, "Product A", 2, 100);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName="Add Order Item Invalid Command")]
        [Trait("Order", "Order Comands")]
        public void AddOrderItemCommand_IsInvalidCommand_ShouldNotPass()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.CustomerIdErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ProductNameErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ProductNameErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.MinQtyErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.PriceErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
        }

        [Fact(DisplayName="Add Order Item Max Quantity Is invalid Command")]
        [Trait("Order", "Order Comands")]
        public void AddOrderItemCommand_MaxQtyIsInvalidCommand_ShouldNotPass()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var orderCommand = new AddOrderItemCommand(customerId, productId, "Product A", Domain.Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            //Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.MaxQtyErrorMsg, orderCommand.ValidationResult.Errors.Select(s => s.ErrorMessage));
        }
    }
}