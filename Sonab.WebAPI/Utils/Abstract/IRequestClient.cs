namespace Sonab.WebAPI.Utils.Abstact;

public interface IRequestClient : IDisposable
{
    Task<T> GetRequestAsync<T>(string url, string token);
    Task<T> PostRequestAsync<T>(string url, Dictionary<string, string> data);
    Task<T> PostRequestAsync<T, Q>(string url, (string, string) header, Q data);
}

