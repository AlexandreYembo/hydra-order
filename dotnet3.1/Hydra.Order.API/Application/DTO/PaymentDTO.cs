namespace Hydra.Order.API.Application.DTO
{
    public class PaymentDTO
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiration { get; set; }
        public string CardCvv { get; set; }
    }
}