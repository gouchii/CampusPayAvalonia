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
        collection.AddScoped<MainWindowViewModel>();
        collection.AddScoped<AuthWindowViewModel>();
        collection.AddScoped<LoginViewModel>();
        collection.AddScoped<SignUpViewModel>();
        collection.AddScoped<HomeViewModel>();
        collection.AddScoped<UserDashBoardViewModel>();
        collection.AddScoped<SettingsViewModel>();
        collection.AddScoped<ProfileViewModel>();

// Register Views
        collection.AddScoped<MainWindow>();
        collection.AddScoped<AuthWindow>();
        collection.AddScoped<LoginView>();
        collection.AddScoped<SignUpView>();
        collection.AddScoped<HomeView>();
        collection.AddScoped<UserDashBoardView>();
        collection.AddScoped<SettingsView>();
        collection.AddScoped<ProfileView>();

// Register Services
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<ViewLocator>();
        collection.AddSingleton<NavigationPageFactory>();
        collection.AddSingleton<WindowManagerService>();
    }
}