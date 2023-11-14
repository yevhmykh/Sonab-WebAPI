using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories;
using Sonab.DbRepositories.Contexts;

namespace Sonab.DbRepositories;

public sealed class DbRepository<TRoot> : IRepository<TRoot> where TRoot : AggregateRoot
{
    private readonly AppDbContext _context;

    public DbRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<TRoot>> GetAsync() => _context.Set<TRoot>().ToListAsync();

    public Task<TRoot> GetByIdAsync(int id) => _context.Set<TRoot>().FirstOrDefaultAsync(root => root.Id == id);

    public Task<TRoot> GetByAsync(Expression<Func<TRoot, bool>> condition) =>
        _context.Set<TRoot>().FirstOrDefaultAsync(condition);

    public Task<List<TRoot>> GetByIdsAsync(IEnumerable<int> ids) =>
        _context.Set<TRoot>().Where(root => ids.Contains(root.Id)).ToListAsync();

    public async Task InsertAsync(TRoot entity) => await _context.AddAsync(entity);

    public Task UpdateAsync(TRoot entity)
    {
        _context.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TRoot entity)
    {
        _context.Remove(entity);
        return Task.CompletedTask;
    }
}
