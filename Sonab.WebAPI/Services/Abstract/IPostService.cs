using Sonab.Core.Dto.Posts;
using Sonab.Core.Dto.Posts.Requests;
using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Services.Abstract;

public interface IPostService
{
    Task<ServiceResponse> GetListAsync(PostSearchType search, PostListParams listParams);
    Task<ServiceResponse> CountAsync(PostSearchType search, PostCountParams countParams);
    Task<ServiceResponse> GetByIdAsync(int id);
    Task<ServiceResponse> CreateAsync(EditRequest request);
    Task<ServiceResponse> UpdateAsync(int id, EditRequest request);
    Task<ServiceResponse> RemoveAsync(int id);
}
