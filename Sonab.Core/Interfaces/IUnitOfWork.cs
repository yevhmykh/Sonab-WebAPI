using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories;

namespace Sonab.Core.Interfaces;

// TODO: Maybe add transaction
public interface IUnitOfWork : IDisposable
{
    Task Commit();
    Task Rollback();
    IRepository<TRoot> GetRepository<TRoot>() where TRoot : AggregateRoot;
}
