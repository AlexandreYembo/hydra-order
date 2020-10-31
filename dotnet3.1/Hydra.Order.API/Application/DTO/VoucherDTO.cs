namespace Hydra.Order.API.Application.DTO
{
    public class VoucherDTO
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public int DiscountType { get; set; }
    }
}