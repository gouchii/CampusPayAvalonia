using System;
using ClientApp.Helpers;
using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentAvalonia.UI.Controls;

namespace ClientApp.ViewModels;

public partial class UserDashBoardViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private readonly TokenManager _tokenManager;


    public UserDashBoardViewModel(NavigationService navigationService, TokenManager tokenManager)
    {
        _navigationService = navigationService;
        _tokenManager = tokenManager;
    }


    // public void OnSelectedNavigationItemChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    // {
    //     var window = WindowHelper.Get();
    //     if (e.SelectedItem is NavigationViewItem nvi)
    //     {
    //         Console.WriteLine($"Navigating to: {nvi.Tag}");
    //         switch (nvi.Tag)
    //         {
    //             case "Home":
    //                 if (window != null) _navigationService.NavigateTo<HomeViewModel>(window, "DashBoardFrame");
    //                 break;
    //             case "Profile":
    //                 if (window != null) _navigationService.NavigateTo<ProfileViewModel>(window, "DashBoardFrame");
    //                 break;
    //             case "Settings":
    //                 if (window != null) _navigationService.NavigateTo<SettingsViewModel>(window, "DashBoardFrame");
    //                 break;
    //             default:
    //                 Console.WriteLine($"No user control implemented yet for {nvi.Tag}");
    //                 break;
    //         }
    //     }
    // }
    public void OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        var window = WindowHelper.Get();

        if (e.InvokedItemContainer is NavigationViewItem nvi)
        {
            Console.WriteLine($"Invoked nav item: {nvi.Tag}");

            switch (nvi.Tag)
            {
                case "Home":
                    if (window != null)
                        _navigationService.NavigateTo<HomeViewModel>(window, "DashBoardFrame");
                    break;
                case "Profile":
                    if (window != null)
                        _navigationService.NavigateTo<ProfileViewModel>(window, "DashBoardFrame");
                    break;
                case "Settings":
                    if (window != null)
                        _navigationService.NavigateTo<SettingsViewModel>(window, "DashBoardFrame");
                    break;
                default:
                    Console.WriteLine($"No user control implemented yet for {nvi.Tag}");
                    break;
            }
        }
    }


}