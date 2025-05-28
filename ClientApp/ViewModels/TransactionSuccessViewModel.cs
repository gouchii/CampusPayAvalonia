using System;
using ClientApp.Contexts;
using ClientApp.Mappers;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels;

public partial class TransactionSuccessViewModel : ViewModelBase
{
    [ObservableProperty] private TransactionModel _successfulTransaction = new();
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;
    private TransactionContext _transactionContext;

    public TransactionSuccessViewModel(TransactionContext transactionContext, IServiceProvider serviceProvider, NavigationService navigationService)
    {
        _transactionContext = transactionContext;
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;

        SuccessfulTransaction = _transactionContext.TransactionDto.ToTransactionModel();
        SuccessfulTransaction.CurrentBalance = transactionContext.CurrentBalance;
    }

    [RelayCommand]
    public void GoBack()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<HomeViewModel>(mainWindow, "DashBoardFrame");
        _transactionContext.Clear();
    }

}