using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface IRepository<TEntity> where TEntity : Key
{
    Task<TEntity> GetByIdAsync(int id);
    Task<bool> AddAndSaveAsync(TEntity entity);
    Task<bool> AddRangeAndSaveAsync(IEnumerable<TEntity> entities);
    Task<bool> UpdateAndSaveAsync(TEntity entity);
    Task<bool> DeleteAndSaveAsync(TEntity entity);
}
