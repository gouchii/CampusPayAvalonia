using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.Contexts;
using ClientApp.Services;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels;

public partial class SendPaymentViewModel : ViewModelBase
{
    private TransactionContext _transactionContext;
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly WindowManagerService _windowManagerService;
    private readonly TransactionService _transactionService;
    [ObservableProperty] private LoadingOverlayViewModel _loadingOverlay;

    public SendPaymentViewModel(TransactionContext transactionContext, NavigationService navigationService, IServiceProvider serviceProvider, WindowManagerService windowManagerService,
        TransactionService transactionService, LoadingOverlayViewModel loadingOverlay)
    {
        _transactionContext = transactionContext;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
        _windowManagerService = windowManagerService;
        _transactionService = transactionService;
        _loadingOverlay = loadingOverlay;
    }

    [RelayCommand]
    private async Task SelectQrSend()
    {
        Console.WriteLine("Selected SendQr");
        _transactionContext.Mode = TransactionMode.SendQr;


        try
        {
            var result = await _windowManagerService.OpenQrWindowAsDialog();
            LoadingOverlay.ShowLoadingOverlay(true);
            Console.WriteLine($"Qr Scanned: {result}");
            if (!string.IsNullOrWhiteSpace(result))
            {
                var transactionDto = await _transactionService.VerifyAsync(result);
                if (transactionDto != null) _transactionContext.TransactionDto = transactionDto;
                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                _navigationService.NavigateTo<TransactionVerificationViewModel>(mainWindow,"DashBoardFrame");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP Error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            LoadingOverlay.ShowLoadingOverlay(false);
        }
    }




[RelayCommand]
private void SelectRfidSend()
{
    Console.WriteLine("Selected SendRfid");
    _transactionContext.Mode = TransactionMode.SendRfid;
}

[RelayCommand]
private void SelectUsernameSend()
{
    Console.WriteLine("Selected SendUsername");
    _transactionContext.Mode = TransactionMode.SendUsername;
}

}