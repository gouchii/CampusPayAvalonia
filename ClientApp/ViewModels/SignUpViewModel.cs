using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ClientApp.Services;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.ViewModels;

public partial class SignUpViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly WindowManagerService _windowManagerService;

    public SignUpViewModel(NavigationService navigationService, WindowManagerService windowManagerService)
    {
        _navigationService = navigationService;
        _windowManagerService = windowManagerService;
    }

    public SignUpViewModel()
    {
    }

    [RelayCommand]
    public void NavigateToLogIn()
    {
        var activeWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime
            ? desktopLifetime.Windows.FirstOrDefault(window => window.IsActive)
            : null;

        // Find the frame in the active window
        var frame = activeWindow?.FindControl<Frame>("AuthFrame");
        if (frame != null)
            _navigationService.NavigateTo<LoginViewModel>(frame,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromRight
                });
    }

}