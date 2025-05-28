using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class UsernameSetView : UserControl
{
    public UsernameSetView(UsernameSetViewModel usernameSetViewModel)
    {
        DataContext = usernameSetViewModel;
        InitializeComponent();
    }
}