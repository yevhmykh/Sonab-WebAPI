using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Services.Abstract;

public interface ISubsriptionService
{
    Task<ServiceResponse> GetAsync();
    Task<ServiceResponse> SubscribeAsync(int publisherId);
    Task<ServiceResponse> UnsubscribeAsync(int publisherId);
}
