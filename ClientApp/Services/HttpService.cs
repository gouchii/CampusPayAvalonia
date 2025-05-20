using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientApp.Services;

public class HttpService
{
    private readonly HttpClient _httpClient;
    private readonly SettingsService _settingsService;

    private string? _jwtToken;
    private const string ApiBaseUrlKey = "ApiBaseUrl";

    public HttpService(SettingsService settingsService)
    {
        _settingsService = settingsService;

        var baseUrl = _settingsService.GetSetting(ApiBaseUrlKey, "http://localhost:5000");
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public void SetBaseUrl(string baseUrl)
    {
        if (!Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
            throw new ArgumentException("Invalid base URL", nameof(baseUrl));

        _settingsService.SetSetting(ApiBaseUrlKey, baseUrl);
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public void SetJwtToken(string token)
    {
        _jwtToken = token;
    }

    public void ClearJwtToken()
    {
        _jwtToken = null;
    }

    private void AttachJwtToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = _jwtToken != null
            ? new AuthenticationHeaderValue("Bearer", _jwtToken)
            : null;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        AttachJwtToken();
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AttachJwtToken();
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task PostAsync<TRequest>(string endpoint, TRequest data)
    {
        AttachJwtToken();
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
    {
        AttachJwtToken();
        var content = CreateJsonContent(data);
        var response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task DeleteAsync(string endpoint)
    {
        AttachJwtToken();
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }


    private static string FormatRoute(string endpointFormat, params object[] args)
    {
        return string.Format(endpointFormat, args);
    }

    private static StringContent CreateJsonContent<T>(T data)
    {
        var json = JsonConvert.SerializeObject(data);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    public async Task<TResponse?> PostWithRouteAsync<TRequest, TResponse>(
        string endpointFormat, object[] routeArgs, TRequest data)
    {
        AttachJwtToken();
        var endpoint = FormatRoute(endpointFormat, routeArgs);
        var content = CreateJsonContent(data);
        var response = await _httpClient.PostAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task<TResponse?> PutWithRouteAsync<TRequest, TResponse>(
        string endpointFormat, object[] routeArgs, TRequest data)
    {
        AttachJwtToken();
        var endpoint = FormatRoute(endpointFormat, routeArgs);
        var content = CreateJsonContent(data);
        var response = await _httpClient.PutAsync(endpoint, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task<TResponse?> GetWithRouteAsync<TResponse>(
        string endpointFormat, params object[] routeArgs)
    {
        AttachJwtToken();
        var endpoint = FormatRoute(endpointFormat, routeArgs);
        var response = await _httpClient.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TResponse>(json);
    }

    public async Task DeleteWithRouteAsync(string endpointFormat, params object[] routeArgs)
    {
        AttachJwtToken();
        var endpoint = FormatRoute(endpointFormat, routeArgs);
        var response = await _httpClient.DeleteAsync(endpoint);
        response.EnsureSuccessStatusCode();
    }
}