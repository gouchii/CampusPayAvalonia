using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SendPaymentView : UserControl
{
    public SendPaymentView(SendPaymentViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}