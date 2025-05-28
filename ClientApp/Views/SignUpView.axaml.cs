using System;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using ClientApp.Helpers;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class SignUpView : UserControl
{

    public SignUpView(SignUpViewModel signUpViewModel)
    {
        DataContext = signUpViewModel;
        InitializeComponent();
    }
}