using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.Contexts;
using ClientApp.Mappers;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels;

public partial class TransactionVerificationViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private TransactionContext _transactionContext;
    private readonly TransactionService _transactionService;
    [ObservableProperty] private TransactionModel _verifiedTransaction = new();

    public TransactionVerificationViewModel(IServiceProvider serviceProvider, NavigationService navigationService, TransactionContext transactionContext, TransactionService transactionService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _transactionContext = transactionContext;
        _transactionService = transactionService;
        VerifiedTransaction = _transactionContext.TransactionDto.ToTransactionModel();
    }

    [RelayCommand]
    public void ReturnHome()
    {
        _transactionContext.Clear();
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<HomeViewModel>(mainWindow, "DashBoardFrame");
    }

    [RelayCommand]
    public async Task ConfirmPayment()
    {
        try
        {
            switch (_transactionContext.Mode)
            {
                case TransactionMode.SendQr:
                    await QrPayment();

                    break;
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
    }

    private async Task QrPayment()
    {
        var transactionResultDto = await _transactionService.ProcessQrPayment(_transactionContext.ToDto());
        if (transactionResultDto != null) _transactionContext.CurrentBalance = transactionResultDto.ScannerBalance;
        _ = _transactionService.LoadAsync();
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<TransactionSuccessViewModel>(mainWindow, "DashBoardFrame");
    }
}