using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientApp.Helpers;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class LoadingOverlayView : UserControl
{
    public LoadingOverlayView()
    {
        InitializeComponent();
    }

}