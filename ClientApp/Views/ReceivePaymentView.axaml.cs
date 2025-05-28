using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class ReceivePaymentView : UserControl
{
    public ReceivePaymentView(ReceivePaymentViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}