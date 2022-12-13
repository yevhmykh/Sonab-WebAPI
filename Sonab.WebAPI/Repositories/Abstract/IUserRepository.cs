using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface IUserRepository : IRepository<User>
{
    Task<User> GetByExternalIdAsync(string externalId);
    Task<User> GetByEmailAsync(string email);
}
