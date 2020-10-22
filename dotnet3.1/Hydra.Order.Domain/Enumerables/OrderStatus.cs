namespace Hydra.Order.Domain.Enumerables
{
    public enum OrderStatus
    {
        Draft = 0,
        Started = 1,
        Processing = 2,
        Processed = 3,
        Delivered = 4,
        Refused = -1,
        Cancelled = -2
    }
}