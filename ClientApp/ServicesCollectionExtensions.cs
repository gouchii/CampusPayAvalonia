using ClientApp.Contexts;
using ClientApp.Services;
using ClientApp.ViewModels;
using ClientApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp;

public static class ServicesCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Register Services
        collection.AddSingleton<NavigationService>();
        collection.AddSingleton<ViewLocator>();
        collection.AddSingleton<NavigationPageFactory>();
        collection.AddSingleton<WindowManagerService>();
        collection.AddSingleton<SettingsService>();
        collection.AddSingleton<ThemeService>();
        collection.AddSingleton<CaptureDeviceManager>();
        collection.AddSingleton<HttpService>();
        collection.AddSingleton<TokenManager>();
        collection.AddSingleton<UserService>();
        collection.AddSingleton<WalletService>();
        collection.AddSingleton<TransactionService>();
        collection.AddSingleton<SignalRService>();
        collection.AddSingleton<AuthService>();

        //Singleton Contexts
        collection.AddSingleton<TransactionContext>();


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
        collection.AddTransient<PlaceHolderViewModel>();
        collection.AddTransient<SendPaymentViewModel>();
        collection.AddTransient<ReceivePaymentViewModel>();
        collection.AddTransient<AmountViewModel>();
        collection.AddTransient<UsernameSetViewModel>();
        collection.AddTransient<RfidScannerWindowViewModel>();
        collection.AddTransient<RfidPinViewModel>();
        collection.AddTransient<QrGeneratorWindowViewModel>();
        collection.AddTransient<TransactionVerificationViewModel>();
        collection.AddTransient<TransactionSuccessViewModel>();

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
        collection.AddTransient<PlaceHolderView>();
        collection.AddTransient<SendPaymentView>();
        collection.AddTransient<ReceivePaymentView>();
        collection.AddTransient<AmountView>();
        collection.AddTransient<UsernameSetView>();
        collection.AddTransient<RfidScannerWindow>();
        collection.AddTransient<RfidPinView>();
        collection.AddTransient<QrGeneratorWindow>();
        collection.AddTransient<TransactionVerificationView>();
        collection.AddTransient<TransactionSuccessView>();
    }
}