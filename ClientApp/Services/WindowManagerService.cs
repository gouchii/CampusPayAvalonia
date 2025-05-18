using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using ClientApp.Helpers;
using ClientApp.ViewModels;
using ClientApp.Views;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Services;

public class WindowManagerService
{
    private readonly Dictionary<string, AppWindow> _activeWindows = new();
    private readonly IServiceProvider _serviceProvider;
    private readonly NavigationService _navigationService;

    public WindowManagerService(IServiceProvider serviceProvider, NavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
    }

    public void RegisterWindow(string windowName, AppWindow window)
    {
        _activeWindows.TryAdd(windowName, window);
        window.Closed += (_, _) => UnregisterWindow(windowName);
    }

    public void UnregisterWindow(string windowName)
    {
        _activeWindows.Remove(windowName);
    }

    public AppWindow? GetActiveWindow(string windowName)
    {
        return _activeWindows.GetValueOrDefault(windowName);
    }

    public void CloseWindow(string windowName)
    {
        var window = GetActiveWindow(windowName);
        UnregisterWindow(windowName);
        window?.Close();
    }

    public void CloseWindow(string windowName, bool isSuccessful)
    {
        var window = GetActiveWindow(windowName);
        UnregisterWindow(windowName);
        window?.Close(isSuccessful);
    }

    public void OpenMainWindow(string windowName = "MainWindow", string frameName = "MainFrame")
    {
        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
        mainWindow.DataContext = mainWindowViewModel;
        mainWindow.InitializeComponent();
        RegisterWindow(windowName, mainWindow);
        if (mainWindow.FindControl<Frame>("MainFrame") is { } mFrame)
        {
            _navigationService.RegisterFrame(mainWindow, mFrame, frameName);
            _navigationService.NavigateTo<UserDashBoardViewModel>(mainWindow, frameName);
        }

        mainWindow.Show();
    }

  public void OpenMainWindowAuthAsDialog(string mainWindowName = "MainWindow",
    string mainFrameName = "MainFrame", string authWindowName = "AuthWindow",
    string authFrameName = "AuthFrame", string userDashBoardFrameName = "DashBoardFrame")
{
    var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
    var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
    mainWindow.DataContext = mainWindowViewModel;
    mainWindow.InitializeComponent();

    mainWindow.SplashScreen = new CustomSplashScreen();
    RegisterWindow(mainWindowName, mainWindow);

    if (mainWindow.FindControl<Frame>(mainFrameName) is { } mainFrame)
    {
        _navigationService.RegisterFrame(mainWindow, mainFrame, mainFrameName);
        _navigationService.NavigateTo<UserDashBoardViewModel>(mainWindow, mainFrameName);
    }

    mainWindow.Show();

    mainWindow.Loaded += async (_, _) =>
    {
        // Check if navigation is already done to avoid multiple calls
        if (mainWindow.MainFrame.Content is not UserDashBoardView)
        {
            return;
        }

        if (mainWindow.MainFrame.Content is UserDashBoardView dashBoardView &&
            dashBoardView.FindControl<Frame>(userDashBoardFrameName) is { } dashBoardFrame)
        {
            _navigationService.RegisterFrame(mainWindow, dashBoardFrame, userDashBoardFrameName);
            _navigationService.NavigateTo<HomeViewModel>(mainWindow, userDashBoardFrameName);
        }

        await Task.Delay(900);

        mainWindow.Effect = new BlurEffect
        {
            Radius = 10
        };

        var authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
        var authWindowViewModel = _serviceProvider.GetRequiredService<AuthWindowViewModel>();
        authWindow.DataContext = authWindowViewModel;
        authWindow.InitializeComponent();
        RegisterWindow(authWindowName, authWindow);

        if (authWindow.FindControl<Frame>(authFrameName) is { } authFrame)
        {
            _navigationService.RegisterFrame(authWindow, authFrame, authFrameName);
            _navigationService.NavigateTo<LoginViewModel>(authWindow, authFrameName);
        }

        var result = await authWindow.ShowDialog<bool>(mainWindow);

        if (!result)
        {
            mainWindow.Close();
        }
        else
        {
            mainWindow.Effect = null;
        }
    };
}



    public void OpenAuthWindow(string windowName = "AuthWindow", string frameName = "AuthFrame")
    {
        var authWindow = _serviceProvider.GetRequiredService<AuthWindow>();
        var authWindowViewModel = _serviceProvider.GetRequiredService<AuthWindowViewModel>();
        authWindow.DataContext = authWindowViewModel;
        authWindow.InitializeComponent();
        RegisterWindow(windowName, authWindow);
        if (authWindow.FindControl<Frame>("AuthFrame") is { } frame)
        {
            _navigationService.RegisterFrame(authWindow, frame, frameName);
            _navigationService.NavigateTo<LoginViewModel>(authWindow, frameName);
        }

        authWindow.SplashScreen = new CustomSplashScreen();
        authWindow.Show();
    }

    public async Task OpenQrWindowAsDialog(string windowName = "QrWindow")
    {
        var qrScannerWindow = _serviceProvider.GetRequiredService<QrScannerWindow>();
        var qrScannerWindowViewModel = _serviceProvider.GetRequiredService<QrScannerWindowViewModel>();
        qrScannerWindow.DataContext = qrScannerWindowViewModel;
        qrScannerWindow.InitializeComponent();
        RegisterWindow(windowName, qrScannerWindow);
        var currentWindow = CurrentWindow.Get();
        if (currentWindow != null) await qrScannerWindow.ShowDialog(currentWindow);
        //     Console.WriteLine("Stopping CameraFeed");
        // };
    }

}