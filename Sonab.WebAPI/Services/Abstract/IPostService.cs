using Sonab.WebAPI.Models;
using Sonab.WebAPI.Models.Posts;

namespace Sonab.WebAPI.Services.Abstract;

public interface IPostService
{
    Task<ServiceResponse> GetListAsync(ListParams listParams);
    Task<ServiceResponse> GetUserPostsAsync(ListParams listParams);
    Task<ServiceResponse> CountAsync();
    Task<ServiceResponse> CountUserPostAsync();
    Task<ServiceResponse> GetByIdAsync(int id);
    Task<ServiceResponse> CreateAsync(EditRequest request);
    Task<ServiceResponse> UpdateAsync(int id, EditRequest request);
    Task<ServiceResponse> RemoveAsync(int id);
}
