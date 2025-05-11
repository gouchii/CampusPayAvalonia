using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;

namespace ClientApp.Views;

public partial class MainWindow : AppWindow
{
    private readonly NavigationService _navigationService;
    public MainWindow(NavigationService navigationService)
    {
        _navigationService = navigationService;
        InitializeComponent();
    }


}
