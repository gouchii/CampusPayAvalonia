using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class RfidPinView : UserControl
{
    public RfidPinView(RfidPinViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}