using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class TransactionSuccessView : UserControl
{
    public TransactionSuccessView(TransactionSuccessViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}