using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using ClientApp.Helpers;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Windowing;
using Microsoft.Extensions.DependencyInjection;
using ZXing.Aztec.Internal;

namespace ClientApp.Views;

public partial class MainWindow : AppWindow
{
    private readonly TokenManager _tokenManager;
    public MainWindow(TokenManager tokenManager, MainWindowViewModel mainWindowViewModel)
    {
        DataContext = mainWindowViewModel;
        _tokenManager = tokenManager;

        InitializeComponent();
        AddHandler(PointerMovedEvent, (_, __) => tokenManager.ResetInactivityTimer(), RoutingStrategies.Tunnel);
        AddHandler(PointerPressedEvent, (_, __) => tokenManager.ResetInactivityTimer(), RoutingStrategies.Tunnel);
        AddHandler(KeyDownEvent, (_, __) => tokenManager.ResetInactivityTimer(), RoutingStrategies.Tunnel);
        // LayoutUpdated += (_, _) =>
        // {
        //     Console.WriteLine($"Window size: {ClientSize.Width} x {ClientSize.Height}");
        // };
    }
}