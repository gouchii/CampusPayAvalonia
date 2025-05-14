using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.ViewModels;
using FluentAvalonia.UI.Controls;

namespace ClientApp.Views;

public partial class UserDashBoardView : UserControl
{
    private readonly UserDashBoardViewModel _viewModel;
    public UserDashBoardView(UserDashBoardViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
        InitializeComponent();
        Console.WriteLine("DashBoard Viewable");
        Menu.SelectedItem = Home;
    }

    private void OnSelectedItemChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        _viewModel.OnSelectedNavigationItemChanged(sender, e);
    }
}