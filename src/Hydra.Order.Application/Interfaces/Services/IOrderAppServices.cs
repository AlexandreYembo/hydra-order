using System.Threading.Tasks;
using Hydra.Order.Application.Models;

namespace Hydra.Order.Application.Interfaces.Services
{
    public interface IOrderAppServices
    {
         Task AddOrder(OrderDto order);
    }
}