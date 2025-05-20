using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.ViewModels;

public partial class LoadingOverlayViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isHitTestVisible;

    [ObservableProperty] private double _opacityValue;

    [ObservableProperty] private bool _isVisible;


    public LoadingOverlayViewModel()
    {
        PropertyChanged += (_, e) => Console.WriteLine($"{e.PropertyName} changed");
    }

    public void ShowLoadingOverlay(bool show)
    {
        Console.WriteLine($"hit set to {IsHitTestVisible}, op set to {OpacityValue}, vis set to {IsVisible}");
        if (show)
        {
            Console.WriteLine("Trying to show loading screen");
            IsHitTestVisible = true;
            OpacityValue = 0.4;
            IsVisible = true;
        }
        else
        {
            IsHitTestVisible = false;
            OpacityValue = 0;
            IsVisible = false;
        }
    }
}