using Sonab.WebAPI.Models.DB;

namespace Sonab.WebAPI.Repositories.Abstract;

public interface IUserRepository : IRepository<User>
{
    Task<bool> CheckIdAsync(string externalId);
    Task<User> GetByEmailAsync(string email);
}
