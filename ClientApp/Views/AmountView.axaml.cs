using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class AmountView : UserControl
{
    public AmountView(AmountViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}