using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class LoginView : UserControl
{

    public LoginView(LoginViewModel loginViewModel)
    {
        DataContext = loginViewModel;
        InitializeComponent();
    }

}