using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using ClientApp.Helpers;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Windowing;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class CustomerWindow : AppWindow
{
    public CustomerWindow()
    {
        InitializeComponent();
    }
}