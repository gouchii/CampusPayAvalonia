using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using ClientApp.Mappers;
using ClientApp.Messages;
using ClientApp.Shared.DTOs.TransactionDto;
using ClientApp.Shared.DTOs.UserDto;
using ClientApp.Shared.DTOs.Wallet;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientApp.Services;

public class SignalRService
{
    private HubConnection? _connection;
    private readonly HttpService _httpService;
    private readonly SettingsService _settingsService;
    private const string ApiBaseUrlKey = "ApiBaseUrl";
    private readonly TransactionService _transactionService;

    public SignalRService(HttpService httpService, SettingsService settingsService, TransactionService transactionService)
    {
        _httpService = httpService;
        _settingsService = settingsService;
        _transactionService = transactionService;
    }

    public async Task StartAsync()
    {
        if (_connection == null)
        {
             InitializeConnection();
        }

        if (_connection != null) await _connection.StartAsync();
        Console.WriteLine("SignalR connected successfully.");
    }

    public async Task StopAsync()
    {
        if (_connection != null)
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _connection = null;
        }
    }

    private void InitializeConnection()
    {
        var baseUrl = _settingsService.GetSetting(ApiBaseUrlKey, "http://localhost:5019");
        var hubUrl = $"{baseUrl}/userhub";

        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_httpService.JwtToken);
            })
            .Build();

        RegisterEventHandlers();
        RegisterReconnectHandlers();
    }

    private void RegisterEventHandlers()
    {
        if (_connection == null) return;
        _connection.On<string>("ReceiveMessage", message => { Console.WriteLine($"SignalR message received: {message}"); });

        _connection.On<UserDto>("UserData", userDto =>
        {
            Console.WriteLine($"Received UserDto: {userDto.UserName}");

            var userModel = userDto.ToUserModel();
            WeakReferenceMessenger.Default.Send(new UserLoadedMessage(userModel));
        });

        _connection.On<TransactionDto>("ReceiveTransaction", dto =>
        {
            Console.WriteLine($"Received Transaction");
            var transaction = dto.ToTransactionModel();
            Console.WriteLine($"Received Transaction: {dto.Amount}, {dto.SenderName}, {dto.ReceiverName}");
            WeakReferenceMessenger.Default.Send(new TransactionReceivedMessage(transaction));
        });

        _connection.On<WalletDto>("ReceiveWalletUpdate", dto =>
        {
            Console.WriteLine($"Received Wallet");
            var wallet = dto.ToWalletModel();
            WeakReferenceMessenger.Default.Send(new WalletLoadedMessage(wallet));
        });

        _connection.On<string>("ReceivePing", fromUserId =>
        {
            Console.WriteLine($"Ping received from user {fromUserId}");
        });
    }

    private void RegisterReconnectHandlers()
    {
        if (_connection == null) return;
        _connection.Closed += async (error) =>
        {
            Console.WriteLine($"SignalR disconnected: {error?.Message}");
            await Task.Delay(2000);
            await StartAsync();
        };

        _connection.Reconnecting += (error) =>
        {
            Console.WriteLine($"SignalR reconnecting: {error?.Message}");
            return Task.CompletedTask;
        };

        _connection.Reconnected += (connectionId) =>
        {
            Console.WriteLine($"SignalR reconnected with connectionId: {connectionId}");
            return Task.CompletedTask;
        };
    }
}
