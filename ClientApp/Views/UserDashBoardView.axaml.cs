using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class UserDashBoardView : UserControl
{
    public UserDashBoardView(UserDashBoardViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        Menu.SelectedItem = Home;
    }
}