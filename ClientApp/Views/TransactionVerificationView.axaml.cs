using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class TransactionVerificationView : UserControl
{
    public TransactionVerificationView(TransactionVerificationViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}