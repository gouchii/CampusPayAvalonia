using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SettingsView : UserControl
{
    public SettingsView(SettingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}