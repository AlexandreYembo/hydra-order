using System.Threading.Tasks;

namespace Hydra.Core.Data
{
    public interface IUnitOfWork
    {
         Task<bool> Commit();
    }
}