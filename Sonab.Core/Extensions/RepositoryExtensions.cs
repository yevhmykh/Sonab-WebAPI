using Sonab.Core.Entities;
using Sonab.Core.Interfaces.Repositories;

namespace Sonab.Core.Extensions;

internal static class RepositoryExtensions
{
    public static Task<User> GetByExternalIdAsync(this IRepository<User> repository, string externalId) =>
        repository.GetByAsync(user => user.ExternalId == externalId);
}
