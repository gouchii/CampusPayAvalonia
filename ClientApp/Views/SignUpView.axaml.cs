using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SignUpView : UserControl
{

    public SignUpView(SignUpViewModel signUpViewModel)
    {
        DataContext = signUpViewModel;
        InitializeComponent();
    }


}