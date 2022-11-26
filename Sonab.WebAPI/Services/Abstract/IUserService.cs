using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Services.Abstract;

public interface IUserService
{
    Task<ServiceResponse> IsLoadedAsync(string externalId);
    void RequestLoading(string externalId);
}
