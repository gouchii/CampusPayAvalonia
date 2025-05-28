using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using ClientApp.Helpers;
using ClientApp.Mappers;
using ClientApp.Models;
using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly WindowManagerService _windowManagerService;
    private readonly HttpService _httpService;
    private readonly AuthService _authService;
    [ObservableProperty] private LoadingOverlayViewModel _loadingOverlay;
    [ObservableProperty] private LoginModel _loginModel = new();
    [ObservableProperty] private bool _isTeachOpen;

    public LoginViewModel(NavigationService navigationService, WindowManagerService windowManagerService, LoadingOverlayViewModel loadingOverlay, HttpService httpService, AuthService authService)
    {
        _navigationService = navigationService;
        _windowManagerService = windowManagerService;
        _httpService = httpService;
        _authService = authService;
        LoadingOverlay = loadingOverlay;
    }


    [RelayCommand]
    public void NavigateToSignIn()
    {
        var activeWindow = WindowHelper.Get();
        var frame = activeWindow?.FindControl<Frame>("AuthFrame");
        if (frame != null)
            _navigationService.NavigateTo<SignUpViewModel>(frame,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                });
    }

    [RelayCommand]
    public async Task LogIn()
    {
        LoginModel.Validate();

        if (LoginModel.HasErrors)
        {
            Console.WriteLine("Validation failed.");
            return;
        }

        LoadingOverlay.ShowLoadingOverlay(true);

        var loginDto = LoginModel.ToLoginRequestDto();

        try
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result)
            {
                _windowManagerService.CloseWindow("AuthWindow", true);
            }
            // _windowManagerService.CloseWindow("AuthWindow", true);
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            LoadingOverlay.ShowLoadingOverlay(false);
        }
    }

    [RelayCommand]
    public void OpenTeach()
    {
        IsTeachOpen = true;
    }



        // LoadingOverlay.ShowLoadingOverlay(true);
        // await Task.Delay(2000);
        // LoadingOverlay.ShowLoadingOverlay(false);
        // _windowManagerService.CloseWindow("AuthWindow", true);
        // _windowManagerService.OpenMainWindow();
        // _windowManagerService.CloseWindow("AuthWindow");
    // }
}