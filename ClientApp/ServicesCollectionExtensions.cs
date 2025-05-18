
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
        collection.AddTransient<AuthWindowViewModel>();
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<SignUpViewModel>();
        collection.AddScoped<HomeViewModel>();
        collection.AddScoped<UserDashBoardViewModel>();
        collection.AddScoped<SettingsViewModel>();
        collection.AddScoped<ProfileViewModel>();
        collection.AddTransient<LoadingOverlayViewModel>();
        collection.AddTransient<QrScannerWindowViewModel>();
        collection.AddTransient<CustomerWindowViewModel>();

// Register Views
        collection.AddScoped<MainWindow>();
        collection.AddTransient<AuthWindow>();
        collection.AddTransient<LoginView>();
        collection.AddTransient<SignUpView>();
        collection.AddScoped<HomeView>();
        collection.AddScoped<UserDashBoardView>();
        collection.AddScoped<SettingsView>();
        collection.AddScoped<ProfileView>();
        collection.AddTransient<LoadingOverlayView>();
        collection.AddTransient<QrScannerWindow>();
        collection.AddTransient<CustomerWindow>();

// Register Services
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<ViewLocator>();
        collection.AddSingleton<NavigationPageFactory>();
        collection.AddSingleton<WindowManagerService>();
        collection.AddSingleton<SettingsService>();
        collection.AddSingleton<ThemeService>();
        collection.AddSingleton<CaptureDeviceManager>();
    }
}