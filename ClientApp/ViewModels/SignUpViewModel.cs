using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ClientApp.Mappers;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Shared.DTOs.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.ViewModels;

public partial class SignUpViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly WindowManagerService _windowManagerService;
    private readonly AuthService _authService;
    private readonly HttpService _httpService;
    [ObservableProperty] private LoadingOverlayViewModel _loadingOverlay;
    [ObservableProperty] private SignUpModel _signUpModel = new();
    [ObservableProperty] private bool _isUserNameTipOpen;
    [ObservableProperty] private string _userNameTipSub;
    [ObservableProperty] private string _userNameTipTitle;

    public SignUpViewModel(NavigationService navigationService,
        WindowManagerService windowManagerService,
        HttpService httpService, LoadingOverlayViewModel loadingOverlay, AuthService authService)
    {
        _navigationService = navigationService;
        _windowManagerService = windowManagerService;
        _httpService = httpService;
        _authService = authService;
        LoadingOverlay = loadingOverlay;
    }

    [RelayCommand]
    public void NavigateToLogIn()
    {
        var activeWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime
            ? desktopLifetime.Windows.FirstOrDefault(window => window.IsActive)
            : null;

        var frame = activeWindow?.FindControl<Frame>("AuthFrame");
        if (frame != null)
            _navigationService.NavigateTo<LoginViewModel>(frame,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
    }


    [RelayCommand]
    public async Task SignUp()
    {
        UserNameTipTitle = "";
        UserNameTipSub = "";
        IsUserNameTipOpen = false;
        SignUpModel.Validate();
        if (SignUpModel.HasErrors)
        {
            Console.WriteLine("Validation failed.");
            return;
        }

        LoadingOverlay.ShowLoadingOverlay(true);

        var signUpDto = SignUpModel.ToSignUpRequestDto();

        try
        {
            var user = await _authService.SignUpAsync(signUpDto);

            if (user)
            {
                _windowManagerService.CloseWindow("AuthWindow", true);
            }
        }
        catch (HttpRequestException httpEx)
        {
            if (httpEx.Message.Contains("Username is already taken"))
            {

                UserNameTipTitle = "Error";
                UserNameTipSub = "Username is already taken";
                IsUserNameTipOpen = true;
            }
            else
            {
                  Console.WriteLine($"HTTP Error: {httpEx.Message}");
            }

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
}