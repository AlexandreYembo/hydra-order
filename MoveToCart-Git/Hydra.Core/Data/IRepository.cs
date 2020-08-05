using System;
using Hydra.Core.DomainObjects;

namespace Hydra.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
         IUnitOfWork UnitOfWork { get; }
    }
}