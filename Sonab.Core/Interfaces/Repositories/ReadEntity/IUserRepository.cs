using Sonab.Core.Entities;

namespace Sonab.Core.Interfaces.Repositories.ReadEntity;

public interface IUserRepository
{
    Task<User> GetByExternalIdAsync(string externalId);
}
