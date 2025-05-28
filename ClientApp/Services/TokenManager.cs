using System;
using System.Threading;
using System.Threading.Tasks;
using ClientApp.Shared.DTOs.Authentication;

namespace ClientApp.Services;

public class TokenManager
{
    private Timer? _refreshTimer;
    private Timer? _inactivityTimer;

    private readonly HttpService _httpService;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _inactivityTimeout = TimeSpan.FromMinutes(1);

    public event Action? OnTimeout;

    public TokenManager(HttpService httpService)
    {
        _httpService = httpService;
    }

    public void Start()
    {
        Console.WriteLine("Starting Token Manager");
        _refreshTimer = new Timer(RefreshTokenCallback, null, _refreshInterval, _refreshInterval);
        ResetInactivityTimer();
    }

    public void Stop()
    {
        _refreshTimer?.Dispose();
        _refreshTimer = null;

        _inactivityTimer?.Dispose();
        _inactivityTimer = null;
    }

    public void ResetInactivityTimer()
    {
        // Console.WriteLine($"Resetting the inactivity timer. Timeout set for {_inactivityTimeout.TotalSeconds}");
        _inactivityTimer?.Dispose();
        _inactivityTimer = new Timer(InactivityTimeoutCallback, null, _inactivityTimeout, Timeout.InfiniteTimeSpan);
    }

    private async void RefreshTokenCallback(object? state)
    {
        try
        {
            if (_httpService.RefreshToken == null) return;
            var refreshRequest = new RefreshTokenRequestDto
            {
                RefreshToken = _httpService.RefreshToken
            };

            var response = await _httpService.PostAsync<RefreshTokenRequestDto, NewUserDto >("api/account/refresh-tokens", refreshRequest);
            if (response != null)
            {
                _httpService.JwtToken = response.AccessToken;
                _httpService.RefreshToken = response.RefreshToken;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }


    private void InactivityTimeoutCallback(object? state)
    {
        Stop();
        OnTimeout?.Invoke();
    }
}
