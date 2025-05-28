using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Windowing;

namespace ClientApp.Views;

public partial class QrGeneratorWindow : AppWindow
{
    public QrGeneratorWindow(QrScannerWindowViewModel viewModel)
    {
        TitleBar.ExtendsContentIntoTitleBar = true;
        DataContext = viewModel;
        InitializeComponent();
    }
}