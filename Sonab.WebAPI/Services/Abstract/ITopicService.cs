using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Services.Abstract;

public interface ITopicService
{
    Task<ServiceResponse> GetListAsync(string namePart);
}
