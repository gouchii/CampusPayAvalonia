using ClientApp.Services;
using ClientApp.ViewModels;
using ClientApp.Views;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp;

public static class ServicesCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Register ViewModels
        collection.AddSingleton<MainWindowViewModel>();
        collection.AddSingleton<AuthWindowViewModel>();
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<SignUpViewModel>();

        // Register Views
        collection.AddSingleton<MainWindow>();
        collection.AddSingleton<AuthWindow>();
        collection.AddTransient<LoginView>();
        collection.AddTransient<SignUpView>();

        // Register Services
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<ViewLocator>();
        collection.AddSingleton<INavigationPageFactory, NavigationPageFactory>();
        collection.AddSingleton<NavigationPageFactory>();
    }
}