using System.Linq.Expressions;
using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories;

/// <summary>
/// CRUD repository
/// </summary>
/// <typeparam name="TRoot">Aggregate Root entity</typeparam>
public interface IRepository<TRoot> where TRoot : AggregateRoot
{
    Task<List<TRoot>> GetAsync();
    Task<TRoot> GetByIdAsync(int id);
    Task<TRoot> GetByAsync(Expression<Func<TRoot, bool>> condition);
    Task<List<TRoot>> GetByIdsAsync(IEnumerable<int> ids);
    Task InsertAsync(TRoot entity);
    Task UpdateAsync(TRoot entity);
    Task DeleteAsync(TRoot entity);
}
