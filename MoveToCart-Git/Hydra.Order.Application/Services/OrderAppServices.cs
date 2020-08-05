using System.Threading.Tasks;
using Hydra.Order.Application.Interfaces.Services;
using Hydra.Order.Application.Models;

namespace Hydra.Order.Application.Services
{
    public class OrderAppServices : IOrderAppServices
    {
        public Task AddOrder(OrderDto order)
        {
            throw new System.NotImplementedException();
        }
    }
}