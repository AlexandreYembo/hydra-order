namespace Hydra.Order.Application.Queries.Dtos
{
    public class OrderPaymentDto
    {
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public string CardCvv { get; set; }
    }
}