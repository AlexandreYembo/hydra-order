namespace Hydra.Order.Domain.Enumerables
{
    public enum OrderStatus
    {
        Draft = 0,
        Started = 1,
        Processed = 4,
        Delivered = 5,
        Cancelled = 6
    }
}