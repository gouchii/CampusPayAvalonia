using ClientApp.Services;
using ClientApp.ViewModels;
using ClientApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp;

public static class ServicesCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Register ViewModels
        collection.AddTransient<MainWindowViewModel>();
        collection.AddTransient<AuthWindowViewModel>();
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<SignUpViewModel>();
        collection.AddTransient<HomeViewModel>();
        collection.AddTransient<UserDashBoardViewModel>();
        collection.AddTransient<SettingsViewModel>();
        collection.AddTransient<ProfileViewModel>();

        // Register Views
        collection.AddTransient<MainWindow>();
        collection.AddTransient<AuthWindow>();
        collection.AddTransient<LoginView>();
        collection.AddTransient<SignUpView>();
        collection.AddTransient<HomeView>();
        collection.AddTransient<UserDashBoardView>();
        collection.AddTransient<SettingsView>();
        collection.AddTransient<ProfileView>();

        // Register Services
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<ViewLocator>();
        collection.AddSingleton<NavigationPageFactory>();
        collection.AddSingleton<WindowManagerService>();
    }
}