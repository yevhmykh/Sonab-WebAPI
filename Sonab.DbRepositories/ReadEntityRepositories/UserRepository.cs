using Microsoft.EntityFrameworkCore;
using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories.ReadEntity;
using Sonab.DbRepositories.Contexts;

namespace Sonab.DbRepositories.ReadEntityRepositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<User> GetByExternalIdAsync(string externalId) =>
        _context.Users.AsTracking().FirstOrDefaultAsync(x => x.ExternalId == externalId);
}
