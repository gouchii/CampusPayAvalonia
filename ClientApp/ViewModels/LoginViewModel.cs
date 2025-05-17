using System.Threading.Tasks;
using Avalonia.Controls;
using ClientApp.Helpers;
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
    [ObservableProperty] private LoadingOverlayViewModel _loadingOverlay;


    public LoginViewModel(NavigationService navigationService, WindowManagerService windowManagerService, LoadingOverlayViewModel loadingOverlay)
    {
        _navigationService = navigationService;
        _windowManagerService = windowManagerService;
        LoadingOverlay = loadingOverlay;
    }

    public LoginViewModel()
    {
        throw new System.NotImplementedException();
    }

    [RelayCommand]
    public async Task NavigateToSignIn()
    {
        LoadingOverlay.ShowLoadingOverlay(true);
        await Task.Delay(2000);
        LoadingOverlay.ShowLoadingOverlay(false);
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
        // LoadingOverlay.ShowLoadingOverlay(true);
        // await Task.Delay(2000);
        // LoadingOverlay.ShowLoadingOverlay(false);
        _windowManagerService.CloseWindow("AuthWindow", true);
        // _windowManagerService.OpenMainWindow();
        // _windowManagerService.CloseWindow("AuthWindow");
    }
}