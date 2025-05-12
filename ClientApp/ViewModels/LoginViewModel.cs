using Avalonia.Controls;
using ClientApp.Helpers;
using ClientApp.Services;
using CommunityToolkit.Mvvm.Input;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ViewModelBase
{

    private readonly NavigationService _navigationService;
    private readonly WindowManagerService _windowManagerService;

    public LoginViewModel(NavigationService navigationService, WindowManagerService windowManagerService)
    {
        _navigationService = navigationService;
        _windowManagerService = windowManagerService;
    }

    public LoginViewModel(WindowManagerService windowManagerService)
    {
        _windowManagerService = windowManagerService;
    }

    public LoginViewModel()
    {
        throw new System.NotImplementedException();
    }

    [RelayCommand]
    public void NavigateToSignIn()
    {
        var activeWindow = CurrentWindow.Get();

        var frame = activeWindow?.FindControl<Frame>("AuthFrame");
        if (frame != null)
            _navigationService.NavigateTo<SignUpViewModel>(frame,
                new SlideNavigationTransitionInfo
                {
                    Effect = SlideNavigationTransitionEffect.FromLeft
                });
    }
    [RelayCommand]
    public void LogIn()
    {
       _windowManagerService.CloseWindow("AuthWindow", true);
       // _windowManagerService.OpenMainWindow();
       // _windowManagerService.CloseWindow("AuthWindow");
    }
}