using System;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Models;
using Xunit;

namespace Hydra.Order.Domain.Tests
{
    public class OrderItemTests
    {
       [Fact(DisplayName= "New Item order with quantity Low than allowed")]
        [Trait("Order", "Order Item")]
        public void AddOrderItem_ItemLowThanAllowedQty_ShouldReturnException()
        {
             //Arrange / Act && Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MIN_QTY_PER_ITEM - 1, 100));
        }
    }
}