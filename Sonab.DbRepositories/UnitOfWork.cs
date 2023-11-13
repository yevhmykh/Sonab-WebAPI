using Sonab.Core.Entities;
using Sonab.Core.Interfaces;
using Sonab.Core.Interfaces.Repositories;
using Sonab.DbRepositories.Contexts;

namespace Sonab.DbRepositories;

// TODO: Maybe add transaction
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly Dictionary<Type, object> _repositories = new();

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public Task Commit() => _context.SaveChangesAsync();

    public Task Rollback() => Task.CompletedTask;

    public IRepository<TRoot> GetRepository<TRoot>() where TRoot : AggregateRoot
    {
        if (_repositories.ContainsKey(typeof(TRoot)))
        {
            return (IRepository<TRoot>)_repositories[typeof(TRoot)];
        }

        var repository = new DbRepository<TRoot>(_context);
        _repositories.Add(typeof(TRoot), repository);
        return repository;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
