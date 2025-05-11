using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Media.Animation;

namespace ClientApp.Views;

public partial class SignUpView : UserControl
{
    private readonly NavigationService _navigationService;
    public SignUpView(NavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }

    private void OnBackClicked(object sender, RoutedEventArgs e)
    {
        _navigationService.NavigateTo<LoginViewModel>(new SlideNavigationTransitionInfo
        {
            Effect = SlideNavigationTransitionEffect.FromRight
        });
    }
}