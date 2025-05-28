using System;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApp.Contexts;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Shared.DTOs.TransactionDto;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels;

public partial class AmountViewModel : ViewModelBase
{
    private TransactionContext _transactionContext;
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private readonly WindowManagerService _windowManagerService;
    private readonly TransactionService _transactionService;
    [ObservableProperty] private AmountModel _amount = new();

    public AmountViewModel(TransactionService transactionService, WindowManagerService windowManagerService, IServiceProvider serviceProvider, NavigationService navigationService,
        TransactionContext transactionContext)
    {
        _transactionService = transactionService;
        _windowManagerService = windowManagerService;
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _transactionContext = transactionContext;
    }

    [RelayCommand]
    public async Task Confirm()
    {
        Amount.Validate();
        if (Amount.HasErrors)
        {
            Console.WriteLine("Validation failed.");
            return;
        }

        switch (_transactionContext.Mode)
        {
            case TransactionMode.ReceiveQr:
                await GenerateQr();
                break;
        }
    }

    [RelayCommand]
    public void GoBack()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<HomeViewModel>(mainWindow, "DashBoardFrame");
    }

    public async Task GenerateQr()
    {
        try
        {
            var refDto = await _transactionService.GenerateTransactionAsync();
            if (refDto != null) _transactionContext.TransactionDto.TransactionRef = refDto.TransactionRef;

            var updateTransactionDto = new UpdateTransactionRequestDto
            {
                Amount = Amount.Amount
            };
             var result = await _transactionService.UpdateTransactionAsync(_transactionContext.TransactionDto.TransactionRef, updateTransactionDto);
             if (result != null)
             {
                 _windowManagerService.OpenQrGeneratorWindowAsDialog(_transactionContext.TransactionDto.TransactionRef);
                 var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                 _navigationService.NavigateTo<HomeViewModel>(mainWindow, "DashBoardFrame");
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
}