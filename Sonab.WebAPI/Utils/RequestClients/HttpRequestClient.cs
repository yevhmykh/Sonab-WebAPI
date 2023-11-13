using System.Net.Http.Headers;
using System.Text.Json;
using Sonab.Core.Interfaces.Services;
using Sonab.WebAPI.Utils.Abstact;

namespace Sonab.WebAPI.Utils.RequestClients;

public class HttpRequestClient : IRequestClient
{
    private readonly HttpClient _client = new();

    public async Task<T> GetRequestAsync<T>(string url, string token)
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        using HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream());
    }

    public async Task<T> PostRequestAsync<T>(string url, Dictionary<string, string> data)
    {
        _client.DefaultRequestHeaders.Clear();
        using FormUrlEncodedContent content = new(data);

        using HttpResponseMessage response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream());
    }

    public async Task<T> PostRequestAsync<T, Q>(string url, (string, string) header, Q data)
    {
        _client.DefaultRequestHeaders.Clear();
        _client.DefaultRequestHeaders.Add(header.Item1, header.Item2);

        using HttpResponseMessage response = await _client.PostAsJsonAsync(url, data);

        return await JsonSerializer.DeserializeAsync<T>(response.Content.ReadAsStream());
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
