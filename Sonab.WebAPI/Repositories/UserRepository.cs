using Microsoft.EntityFrameworkCore;
using Sonab.WebAPI.Contexts;
using Sonab.WebAPI.Models.DB;
using Sonab.WebAPI.Repositories.Abstract;

namespace Sonab.WebAPI.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public Task<bool> CheckIdAsync(string externalId) =>
        _context.Users.AnyAsync(x => x.ExternalId == externalId);

    public Task<User> GetByEmailAsync(string email) =>
        _context.Users.FirstOrDefaultAsync(x => x.Email == email.ToUpper());
}
