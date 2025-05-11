using Avalonia.Controls;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ClientApp.Views;

public partial class AuthWindow : AppWindow
{
    public AuthWindow(AuthWindowViewModel viewModel,
        NavigationService navigationService,
        INavigationPageFactory navigationPageFactory)
    {
        DataContext = viewModel;
        InitializeComponent();

        if (this.FindControl<Frame>("AuthFrame") is { } frame)
        {
            navigationService.SetFrame(frame);
            frame.NavigationPageFactory = navigationPageFactory;
            navigationService.NavigateTo<LoginViewModel>();
        }
    }
}

