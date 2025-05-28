using System;
using Avalonia.Controls;
using ClientApp.Helpers;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class LoginView : UserControl
{

    public LoginView(LoginViewModel loginViewModel)
    {
        DataContext = loginViewModel;
        InitializeComponent();
    }

}