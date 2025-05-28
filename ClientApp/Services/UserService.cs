using System;
using System.Threading.Tasks;
using ClientApp.Mappers;
using ClientApp.Messages;
using ClientApp.Models;
using ClientApp.Shared.DTOs.UserDto;
using CommunityToolkit.Mvvm.Messaging;

namespace ClientApp.Services;

public class UserService
{
    private readonly HttpService _httpService;
    public UserModel? UserModel { get; set; } = new();

    public UserService(HttpService httpService)
    {
        _httpService = httpService;
        Console.WriteLine("UserService created and registering message handlers.");

        WeakReferenceMessenger.Default.Register<UserLoggedInMessage>(this, (obj, s) =>
        {
            Console.WriteLine("UserLoggedInMessage received in UserService.");
            _ = LoadAsync();
        });
        WeakReferenceMessenger.Default.Register<UserLoggedOutMessage>(this, (obj, s) =>
        {
            Console.WriteLine("UserLoggedOutMessage received in UserService.");
            Clear();
        });
    }


    public async Task LoadAsync(int maxRetries = 3)
    {
        int attempt = 0;
        while (attempt < maxRetries)
        {
            try
            {
                var result = await _httpService.GetAsync<UserDto>("/api/user");
                if (result != null)
                {
                    UserModel = result.ToUserModel();
                    Console.WriteLine($"User data loaded: {UserModel?.UserName}");
                    if (UserModel != null) WeakReferenceMessenger.Default.Send(new UserLoadedMessage(UserModel));
                    return;
                }

                Console.WriteLine("Received null user DTO from API.");
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

        UserModel = null;
        Console.WriteLine("Failed to load user data after retries.");
    }


    public void Clear()
    {
        UserModel = null;
        Console.WriteLine("User data cleared.");
    }
}