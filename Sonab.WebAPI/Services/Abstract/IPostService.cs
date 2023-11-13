using Sonab.Core.Dto.Posts;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Services.Abstract;

public interface IPostService
{
    Task<ServiceResponse> GetListAsync(SearchType search, PostListParams listParams);
    Task<ServiceResponse> CountAsync(SearchType search, PostCountParams countParams);
    Task<ServiceResponse> GetByIdAsync(int id);
    Task<ServiceResponse> CreateAsync(EditRequest request);
    Task<ServiceResponse> UpdateAsync(int id, EditRequest request);
    Task<ServiceResponse> RemoveAsync(int id);
}
