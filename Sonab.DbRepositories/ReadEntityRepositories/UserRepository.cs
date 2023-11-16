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

    public Task<User> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(x => x.Email == email.ToUpper());

    public Task<User> GetByExternalIdAsync(string externalId) =>
        _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ExternalId == externalId);
}
