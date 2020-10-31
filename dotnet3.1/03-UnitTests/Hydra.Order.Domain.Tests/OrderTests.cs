using System;
using System.Linq;
using Hydra.Core.DomainObjects;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Models;
using Xunit;

namespace Hydra.Order.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName= "Add Item new Order")]
        [Trait("Order", "Order")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 2, 100);

            //Act
            order.AddItem(orderItem);

            //Assert
            Assert.Equal(200, order.Amount);
        }

        [Fact(DisplayName= "Add Item existing order")]
        [Trait("Order", "Order")]
        public void AddOrderItem_ExistingItem_ShouldIncrementQtyAndUpdateValue()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", 1, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(300, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault( p => p.ProductId == productId).Qty);
        }

        [Fact(DisplayName= "New Item order with quantity Greater than allowed")]
        [Trait("Order", "Order")]
        public void AddOrderItem_ItemGreaterThanAllowedQty_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.AddItem(orderItem));
        }

        [Fact(DisplayName= "Existing Item order with quantity Greater than allowed")]
        [Trait("Order", "Order")]
        public void AddOrderItem_ExistingItemGreaterThanAllowedQty_ShouldReturnException()
        {
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Product", Models.Order.MAX_QTY_PER_ITEM, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.AddItem(orderItem2));
        }

        [Fact(DisplayName= "Update item existing order")]
        [Trait("Order", "Order")]
        public void UpdateOrderItem_ItemDoesNotExist_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.UpdateItem(orderItem));
        }

        [Fact(DisplayName= "Existing Valid Item")]
        [Trait("Order", "Order")]
        public void ExistingOrderItem_ValidItem_ShouldAllowToUpdateQty()
        {
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(productId, "Test Product", 1, 100);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(productId, "Test Product", 5, 100);
            var newQty = orderItemUpdated.Qty;

            //Act
            order.UpdateItem(orderItemUpdated);

            //Assert
            Assert.Equal(newQty, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Qty);
        }

        [Fact(DisplayName= "Existing Valid Item Validate Amount")]
        [Trait("Order", "Order")]
        public void ExistingOrderItem_DifferentItems_ShouldAllowToUpdateAmount()
        {
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var orderItem1 = new OrderItem(productId1, "Test Product A", 2, 100);
            var orderItem2 = new OrderItem(productId2, "Test Product B", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var orderItemUpdated = new OrderItem(productId2, "Test Product B", 5, 15);
            var amountItems = orderItem1.Qty * orderItem1.Price + orderItemUpdated.Qty * orderItemUpdated.Price;

            //Act
            order.UpdateItem(orderItemUpdated);

            //Assert
            Assert.Equal(amountItems, order.Amount);
        }

        [Fact(DisplayName= "Update item qty greater than allowed")]
        [Trait("Order", "Order")]
        public void UpdateOrderItem_ItemQtyGreaterThanAllowed_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(Guid.NewGuid(), "Test Product", Models.Order.MAX_QTY_PER_ITEM + 1, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.UpdateItem(orderItemUpdated));
        }

        [Fact(DisplayName= "Remove item from order does not exist")]
        [Trait("Order", "Order")]
        public void RemoveOrderItem_ItemDoesNotExist_ShouldReturnException()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId = Guid.NewGuid();

            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product", 3, 15);

            // Act && Assert
            Assert.Throws<DomainException>(() =>  order.RemoveItem(orderItem));
        }

        [Fact(DisplayName= "Remove Item Should Update Amount")]
        [Trait("Order", "Order")]
        public void RemoveOrderItem_ExistingItem_ShouldAllowToUpdateAmount()
        {
             //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());

            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var orderItem1 = new OrderItem(productId1, "Test Product A", 2, 100);
            var orderItem2 = new OrderItem(productId2, "Test Product B", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var amountItems = orderItem2.Qty * orderItem2.Price;

            //Act
            order.RemoveItem(orderItem1);

            //Assert
            Assert.Equal(amountItems, order.Amount);
        }

        [Fact(DisplayName = "Apply valid voucher")]
        [Trait("Order", "Order")]
        public void ApplyVoucher_ApplyValidVoucher_ShouldReturnValid()
        {
              //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var voucher = new Voucher("ALEX-10", null, 15, 1, VoucherType.Value, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Apply invalid voucher")]
        [Trait("Order", "Order")]
        public void ApplyVoucher_ApplyInValidVoucher_ShouldReturnInValid()
        {
              //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var voucher = new Voucher("ALEX-10", null, 15, 1, VoucherType.Value, DateTime.Now.AddDays(-1), true, true);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Apply Discount Value Type voucher")]
        [Trait("Order", "Order")]
        public void ApplyVoucher_ApplyTypeDiscountValueVoucher_ShouldApplyDiscount()
        {
              //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test Product A", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product B", 3, 15);

            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var voucher = new Voucher("ALEX-10", null, 15, 1, VoucherType.Value, DateTime.Now.AddDays(15), true, false);

            var discountApplied = order.Amount - voucher.Discount;
            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(discountApplied, order.Amount);
        }

        [Fact(DisplayName = "Apply Discount Percentage Type voucher")]
        [Trait("Order", "Order")]
        public void Apply_Voucher_ApplyTypeDiscountPercentageVoucher_ShouldApplyDiscount()
        {
              //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test Product A", 2, 100);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product B", 3, 15);

            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var voucher = new Voucher("ALEX-10", 15, null, 1, VoucherType.Percentage, DateTime.Now.AddDays(15), true, false);

            var discountApplied = (order.Amount * voucher.Discount) / 100;

            var amountWithDiscount = order.Amount - discountApplied;
            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(amountWithDiscount, order.Amount);
        }

        [Fact(DisplayName = "Apply discount voucher exceded amount")]
        [Trait("Order", "Order")]
        public void ApplyVoucher_ExcededAmount_OrderShouldHasAmountZero()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Product A", 2, 100);
            
            order.AddItem(orderItem);

            var voucher = new Voucher("ALEX-10", null, 300, 1, VoucherType.Value, DateTime.Now.AddDays(15), true, false);
           
            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(0, order.Amount);
        }

        [Fact(DisplayName = "Apply voucher should recalculate on update order")]
        [Trait("Order", "Order")]
        public void ApplyVoucher_ChangeOrderItem_ShouldCalculateAmountDiscount()
        {
            //Arrange
            var order = Models.Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Test Product A", 2, 100);
            
            order.AddItem(orderItem1);

            var voucher = new Voucher("ALEX-10", null, 50, 1, VoucherType.Value, DateTime.Now.AddDays(15), true, false);
            order.ApplyVoucher(voucher);

            var orderItem2 = new OrderItem(Guid.NewGuid(), "Test Product B", 4, 25);

            //Act
            order.AddItem(orderItem2);
        
            //Assert
            var expectedAmount = order.OrderItems.Sum(s => s.Qty * s.Price) - voucher.Discount;

            Assert.Equal(expectedAmount, order.Amount);
        }
    }
}
