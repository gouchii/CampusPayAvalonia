using System;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class SettingsView : UserControl
{

    public SettingsView(SettingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        var dc = DataContext as SettingsViewModel;
        Console.WriteLine($"Set Data context as {dc?.GetType()} in settings view");
        if (!TryGetResource("SystemAccentColor", null, out var value)) return;
        if (value == null) return;
        var color = Unsafe.Unbox<Color>(value);
        if (dc == null) return;
        dc.CustomAccentColor = color;
        dc.ListBoxColor = color;
        Console.WriteLine($"Set accent color as {color} in settings view");
    }
}