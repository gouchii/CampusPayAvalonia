using System;
using System.Threading.Tasks;
using ClientApp.Messages;
using ClientApp.Shared.DTOs.Authentication;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientApp.Services;

public class AuthService
{
    private readonly HttpService _httpService;
    private readonly TokenManager _tokenManager;
    private readonly WalletService _walletService;
    private readonly UserService _userService;
    private readonly SignalRService _signalR;
    private readonly TransactionService _transactionService;

    public AuthService(HttpService httpService, TokenManager tokenManager, UserService userService, SignalRService signalR, WalletService walletService, TransactionService transactionService)
    {
        _httpService = httpService;
        _tokenManager = tokenManager;
        _userService = userService;
        _signalR = signalR;
        _walletService = walletService;
        _transactionService = transactionService;

        _tokenManager.OnTimeout += () => { Logout(); };
    }

    public async Task<bool> LoginAsync(LoginRequestDto loginDto)
    {
        var user = await _httpService.PostAsync<LoginRequestDto, NewUserDto>("/api/account/login", loginDto);

        if (user != null && !string.IsNullOrWhiteSpace(user.AccessToken))
        {
            _httpService.JwtToken = user.AccessToken;
            _httpService.RefreshToken = user.RefreshToken;
            Console.WriteLine($"Logged in: {user.UserName}");
            _tokenManager.Start();
            await _signalR.StartAsync();
            WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
            return true;
        }

        Console.WriteLine("Login failed: No user returned.");
        return false;
    }

    public async Task<bool> SignUpAsync(SignUpRequestDto signUpDto)
    {
        var user = await _httpService.PostAsync<SignUpRequestDto, NewUserDto>("/api/account/signup", signUpDto);

        if (user != null && !string.IsNullOrWhiteSpace(user.AccessToken))
        {
            _httpService.JwtToken = user.AccessToken;
            _httpService.RefreshToken = user.RefreshToken;
            Console.WriteLine("Sign Up Successful");
            Console.WriteLine($"Logged in: {user.UserName}");
            _tokenManager.Start();
            WeakReferenceMessenger.Default.Send(new UserLoggedInMessage());
            await _signalR.StartAsync();
            return true;
        }

        Console.WriteLine("Login failed: No user returned.");
        return false;
    }

    public void Logout()
    {
        _httpService.ClearJwtToken();
        _httpService.RefreshToken = null;
        _tokenManager.Stop();
        WeakReferenceMessenger.Default.Send(new UserLoggedOutMessage());
        _ = _signalR.StopAsync();
    }
}