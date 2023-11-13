namespace Sonab.Core.Interfaces.Services;

public interface IRequestClient : IDisposable
{
    Task<TResponse> GetRequestAsync<TResponse>(string url, string token);
    Task<TResponse> PostRequestAsync<TResponse>(string url, Dictionary<string, string> data);
    Task<TResponse> PostRequestAsync<TResponse, TRequest>(string url, (string, string) header, TRequest data);
}

