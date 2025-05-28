using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ClientApp.Helpers;
using ClientApp.Services;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class UserDashBoardView : UserControl
{
    private readonly UserDashBoardViewModel _viewModel;
    public UserDashBoardView(UserDashBoardViewModel viewModel, TokenManager tokenManager)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        Console.WriteLine("DashBoard Viewable");
        Menu.SelectedItem = Home;
        tokenManager.OnTimeout += () =>
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Menu.SelectedItem = Home;
            });
        };
    }

    // private void OnSelectedItemChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    // {
    //     _viewModel.OnSelectedNavigationItemChanged(sender, e);
    // }

    private void Menu_OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        _viewModel.OnItemInvoked(sender, e);
    }
}