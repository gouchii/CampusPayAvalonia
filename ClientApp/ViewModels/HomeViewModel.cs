using System;
using System.Collections.ObjectModel;
using System.Linq;
using ClientApp.Messages;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Shared.Enums.Transaction;
using ClientApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;


namespace ClientApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly WindowManagerService _windowManagerService;
    private readonly UserService _userService;
    private readonly WalletService _walletService;
    private readonly TransactionService _transactionService;
    private readonly NavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;


    [ObservableProperty] private UserModel? _userModel = new();

    [ObservableProperty] private WalletModel? _walletModel = new();


    [ObservableProperty] private ObservableCollection<TransactionModel> _recentTransaction = new();

    [ObservableProperty] private TransactionModel? _selectedTransaction;
    [ObservableProperty] private string _testBox;

    public HomeViewModel(WindowManagerService windowManagerService, UserService userService, WalletService walletService, TransactionService transactionService, NavigationService navigationService,
        IServiceProvider serviceProvider)
    {
        _windowManagerService = windowManagerService;
        _userService = userService;
        _walletService = walletService;
        _transactionService = transactionService;
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;


        WeakReferenceMessenger.Default.Register<UserLoadedMessage>(this, (r, m) =>
        {
            Console.WriteLine("Loading UserModel in HomeView");
            UserModel = m.Value;
        });

        if (_userService.UserModel == null) return;
        Console.WriteLine("UserModel already loaded, setting immediately");
        UserModel = _userService.UserModel;

        WeakReferenceMessenger.Default.Register<WalletLoadedMessage>(this, (r, m) =>
        {
            WalletModel = m.Value;
            Console.WriteLine($"Loading WalletModel in HomeView : {WalletModel.Balance}");
        });

        if (_walletService.WalletModel == null) return;
        WalletModel = _walletService.WalletModel;
        Console.WriteLine($"WalletModel already loaded, setting immediately : Balance({WalletModel.Balance})");

        WeakReferenceMessenger.Default.Register<TransactionsLoadedMessage>(this, (r, message) =>
        {
            Console.WriteLine($"Loading Transactions in HomeView");
            RecentTransaction.Clear();

            foreach (var transaction in message.Transactions)
            {
                var isReceiver = transaction.ReceiverName == UserModel?.UserName;

                transaction.DisplayTitle = isReceiver
                    ? transaction.SenderName
                    : transaction.ReceiverName;

                Console.WriteLine($"Display Titles :{transaction.DisplayTitle} | {transaction.ReceiverName} | {transaction.SenderName}");
                transaction.DisplayAmount = (isReceiver ? "+" : "-") + transaction.Amount.ToString("0.00");

                RecentTransaction.Add(transaction);
            }
        });

        if (_transactionService.Transactions.Count == 0) return;
        {
            Console.WriteLine("Transactions already loaded, setting immediately");
            RecentTransaction.Clear();

            foreach (var transaction in _transactionService.Transactions)
            {
                var isReceiver = transaction.ReceiverName == UserModel?.UserName;

                transaction.DisplayTitle = isReceiver
                    ? transaction.SenderName
                    : transaction.ReceiverName;

                transaction.DisplayAmount = (isReceiver ? "+" : "-") + transaction.Amount.ToString("0.00");

                Console.WriteLine($"Display Titles : {transaction.DisplayTitle} | {transaction.ReceiverName} | {transaction.SenderName}");
                RecentTransaction.Add(transaction);
            }
        }

        WeakReferenceMessenger.Default.Register<TransactionReceivedMessage>(this, (r, message) =>
        {
            var newTransaction = message.Value;


            if (newTransaction.Status == TransactionStatus.Completed)
            {
                RecentTransaction.Insert(0, newTransaction);
            }

            Console.WriteLine($"Received live transaction: {newTransaction.DisplayTitle}, {newTransaction.DisplayAmount}");
        });
    }

    partial void OnSelectedTransactionChanged(TransactionModel? value)
    {
        if (value is not null)
        {
            // Handle click
            Console.WriteLine($"Clicked on: {value.ReceiverName} - {value.Amount}");
        }
    }

    [RelayCommand]
    public void OpenScanDialog()
    {
        _ = _windowManagerService.OpenQrWindowAsDialog();
    }

    [RelayCommand]
    public void OpenCustomerWindow()
    {
        _windowManagerService.OpenCustomerWindow();
    }

    [RelayCommand]
    public void NavigateToSendView()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<SendPaymentViewModel>(mainWindow, "DashBoardFrame");
    }

    partial void OnTestBoxChanged(string value)
    {
    }

    [RelayCommand]
    public void NavigateToReceiveView()
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        _navigationService.NavigateTo<ReceivePaymentViewModel>(mainWindow, "DashBoardFrame");
    }

    [RelayCommand]
    public void ReloadTransaction()
    {
        _ = _transactionService.LoadAsync();
    }

    [RelayCommand]
    public void ReloadWallet()
    {
        _ = _walletService.LoadAsync();
    }

}

