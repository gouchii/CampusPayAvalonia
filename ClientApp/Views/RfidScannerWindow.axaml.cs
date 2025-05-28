using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace ClientApp.Views;

public partial class RfidScannerWindow : AppWindow
{
    public RfidScannerWindow(RfidScannerWindowViewModel windowViewModel)
    {
        DataContext = windowViewModel;
        InitializeComponent();
    }
}