using Microsoft.EntityFrameworkCore;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Repositories.Abstract;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Key
{
    protected readonly AppDbContext _context;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<TEntity> GetByIdAsync(int id) => _context
        .Set<TEntity>()
        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> AddAndSaveAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddRangeAndSaveAsync(IEnumerable<TEntity> entities)
    {
        await _context.AddRangeAsync(entities);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAndSaveAsync(TEntity entity)
    {
        _context.Update(entity);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAndSaveAsync(TEntity entity)
    {
        _context.Remove(entity);
        return await _context.SaveChangesAsync() > 0;
    }
}
