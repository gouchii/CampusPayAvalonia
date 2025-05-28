using System;
using System.Threading.Tasks;
using ClientApp.Mappers;
using ClientApp.Messages;
using ClientApp.Models;
using ClientApp.Shared.DTOs.UserDto;
using ClientApp.Shared.DTOs.Wallet;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientApp.Services;

public class WalletService
{
    private readonly HttpService _httpService;
    public WalletModel? WalletModel { get; set; } = new();

    public WalletService(HttpService httpService)
    {
        _httpService = httpService;
        Console.WriteLine("WalletService created and registering message handlers.");
        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this, (obj, s) =>
        {
            Console.WriteLine("UserLoggedInMessage received in WalletService.");
            _ = LoadAsync();
        });
        WeakReferenceMessenger.Default.Register<UserLoggedOutMessage>(this, (obj, s) =>
        {
            Console.WriteLine("UserLoggedOutMessage received in WalletService.");
            Clear();
        });
    }


    public async Task LoadAsync(int maxRetries = 3)
    {
        var attempt = 0;
        while (attempt < maxRetries)
        {
            try
            {
                var result = await _httpService.GetAsync<WalletDto>("/api/wallet");
                if (result != null)
                {
                    WalletModel = result.ToWalletModel();
                    Console.WriteLine($"Wallet data loaded : wallet model ={WalletModel.Balance} dto = {result.Balance} ");
                    if (WalletModel != null) WeakReferenceMessenger.Default.Send(new WalletLoadedMessage(WalletModel));
                    return;
                }

                Console.WriteLine("Received null wallet DTO from API.");
                break;
            }
            catch (Exception ex)
            {
                attempt++;
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");

                if (attempt == maxRetries)
                    break;

                await Task.Delay(TimeSpan.FromSeconds(1 << attempt));
            }
        }

        WalletModel = null;
        Console.WriteLine("Failed to load user data after retries.");
    }


    public void Clear()
    {
        WalletModel = null;
        Console.WriteLine("User data cleared.");
    }
}