using System;
using System.Linq;
using Hydra.Order.Domain.Enumerables;
using Hydra.Order.Domain.Models;
using Hydra.Order.Domain.Validations;
using Xunit;

namespace Hydra.Order.Domain.Tests
{
    //This validation is applied in the order level (aggreate root)
     public class VoucherTests
     {
    //     [Fact(DisplayName="Validate voucher valid value type")]
    //     [Trait("Voucher", "Order - Voucher")]
    //     public void Voucher_ValidateVoucherValueType_ShouldBeValid(){
    //         //Arrange
    //         var voucher = new Voucher("ALEX-10",
    //             null,
    //             15,
    //             1,
    //             VoucherType.Value,
    //             DateTime.Now.AddDays(15),
    //             true,
    //             false);
    //         //Act
    //         var result = voucher.IsValid();
            
    //         //Assert 
    //         Assert.True(result.IsValid);
    //     }

    //     [Fact(DisplayName="Validate voucher invalid value type")]
    //     [Trait("Voucher", "Order - Voucher")]
    //     public void Voucher_ValidateVoucherValueType_ShouldBeInvalid(){
    //         //Arrange
    //         var voucher = new Voucher("",
    //             null,
    //             null,
    //             0,
    //             VoucherType.Value,
    //             DateTime.Now.AddDays(-1),
    //             false,
    //             true);
           
    //         //Act
    //         var result = voucher.IsValid();
            
    //         //Assert 
    //         Assert.False(result.IsValid);
    //         Assert.Equal(6, result.Errors.Count);
    //         Assert.Contains(VoucherValidation.InactiveVoucherError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.CodeError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.VoucherExpiredError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.QtyError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.UsedVoucherError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.DiscountValueError, result.Errors.Select(s => s.ErrorMessage));
    //     }

    //     [Fact(DisplayName="Validate voucher valid percentage type")]
    //     [Trait("Voucher", "Order - Voucher")]
    //     public void Voucher_ValidateVoucherPercentageType_ShouldBeValid(){
    //         //Arrange
    //         var voucher = new Voucher("ALEX-10",
    //             15,
    //             null,
    //             1,
    //             VoucherType.Percentage,
    //             DateTime.Now.AddDays(15),
    //             true,
    //             false);
            
    //         //Act
    //         var result = voucher.IsValid();
            
    //         //Assert 
    //         Assert.True(result.IsValid);
    //     }

    //     [Fact(DisplayName="Validate voucher invalid percentage type")]
    //     [Trait("Voucher", "Order - Voucher")]
    //     public void Voucher_ValidateVoucherPercentageType_ShouldBeInvalid(){
    //         //Arrange
    //         var voucher = new Voucher("",
    //             null,
    //             null,
    //             0,
    //             VoucherType.Percentage,
    //             DateTime.Now.AddDays(-1),
    //             false,
    //             true);
           
    //         //Act
    //         var result = voucher.IsValid();
            
    //         //Assert 
    //         Assert.False(result.IsValid);
    //         Assert.Equal(6, result.Errors.Count);
    //         Assert.Contains(VoucherValidation.InactiveVoucherError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.CodeError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.VoucherExpiredError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.QtyError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.UsedVoucherError, result.Errors.Select(s => s.ErrorMessage));
    //         Assert.Contains(VoucherValidation.PercentageDiscountError, result.Errors.Select(s => s.ErrorMessage));
    //     }
    }
}