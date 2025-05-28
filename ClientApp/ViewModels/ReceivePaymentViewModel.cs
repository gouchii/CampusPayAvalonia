using System;
using ClientApp.Contexts;
using ClientApp.Services;
using ClientApp.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels;

public partial class ReceivePaymentViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly NavigationService _navigationService;
    private TransactionContext _transactionContext;

    public ReceivePaymentViewModel(IServiceProvider serviceProvider, NavigationService navigationService, TransactionContext transactionContext)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
        _transactionContext = transactionContext;
        _transactionContext.Clear();
    }

    [RelayCommand]
    private void NavigateToAmountView()
    {
        _transactionContext.Mode = TransactionMode.ReceiveQr;
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<AmountViewModel>(mainWindow,"DashBoardFrame");
    }
}