using Avalonia.Controls;
using Avalonia.Interactivity;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.Views;

public partial class LoginView : UserControl
{
    private readonly NavigationService _navigationService;
    public LoginView(NavigationService navigationService)
    {
        InitializeComponent();
        _navigationService = navigationService;
    }

    private void OnRegistrationClicked(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateTo<SignUpViewModel>("AuthFrame",
            new SlideNavigationTransitionInfo
        {
            Effect = SlideNavigationTransitionEffect.FromLeft
        });
    }
}