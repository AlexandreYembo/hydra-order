using System;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Models;
using Xunit;

namespace Hydra.Order.Domain.Tests
{
    public class OrderItemTests
    {
[       Fact(DisplayName= "New Item order with quantity Greater than allowed")]
        [Trait("Order", "Order Tests")]
        public void AddOrderItem_ItemGreaterThanAllowedQty_ShouldReturnException()
        {
             //Arrange / Act && Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100));
        }

        [Fact(DisplayName= "New Item order with quantity Low than allowed")]
        [Trait("Order", "Order Item Tests")]
        public void AddOrderItem_ItemLowThanAllowedQty_ShouldReturnException()
        {
            //Arrange / Act && Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MIN_QTY_PER_ITEM - 1, 100));
        }
    }
}