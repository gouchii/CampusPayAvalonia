using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using FluentAvalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly FluentAvaloniaTheme? _faTheme;

    [ObservableProperty]
    private bool _useCustomAccent;

    [ObservableProperty]
    private Color _customAccentColor = Colors.SlateBlue;

    [ObservableProperty]
    private string _currentAppTheme = _system;

    [ObservableProperty]
    private Color? _listBoxColor;

    public SettingsViewModel()
    {
        GetPredefColors();
        _faTheme = Application.Current?.Styles[0] as FluentAvaloniaTheme;
        Console.WriteLine($"Theme set to {_faTheme}");
    }


    public List<Color> PredefinedColors { get; private set; }

    public string? CurrentVersion =>
        typeof(FluentAvalonia.UI.Controls.NavigationView).Assembly.GetName().Version?.ToString();

    public string CurrentAvaloniaVersion =>
        typeof(Application).Assembly.GetName().Version?.ToString() ?? throw new InvalidOperationException();

    public string[] AppThemes { get; } =
        new[] { _system, _light , _dark };

    partial void OnUseCustomAccentChanged(bool value)
    {

        if (value)
        {
            if (_faTheme != null && _faTheme.TryGetResource("SystemAccentColor", null, out var curColor))
            {
                Console.WriteLine($"Tried to change the accent color to {CustomAccentColor}");
                CustomAccentColor = (Color)curColor;
                ListBoxColor = CustomAccentColor;
            }
            else
            {
                throw new Exception("Unable to retrieve SystemAccentColor");
            }
        }
        else
        {
            CustomAccentColor = default;
            ListBoxColor = default;
            UpdateAppAccentColor(null);
        }
    }

    partial void OnListBoxColorChanged(Color? value)
    {
        CustomAccentColor = value ?? default;
        UpdateAppAccentColor(CustomAccentColor);
    }
    partial void OnCustomAccentColorChanged(Color value)
    {
        ListBoxColor = value;
        UpdateAppAccentColor(value);
    }

    partial void OnCurrentAppThemeChanged(string value)
    {
        var newTheme = GetThemeVariant(value);
        if (newTheme != null)
        {
            Application.Current!.RequestedThemeVariant = newTheme;
        }
        if (_faTheme != null) _faTheme.PreferSystemTheme = (value == _system);
    }

    private ThemeVariant? GetThemeVariant(string value) => value switch
    {
        _light => ThemeVariant.Light,
        _dark => ThemeVariant.Dark,
        _system => null,
        _ => null,
    };

    private void GetPredefColors()
    {
        PredefinedColors = new List<Color>
        {
            Color.FromRgb(255,185,0),
            Color.FromRgb(255,140,0),
            Color.FromRgb(247,99,12),
            Color.FromRgb(202,80,16),
            Color.FromRgb(218,59,1),
            Color.FromRgb(239,105,80),
            Color.FromRgb(209,52,56),
            Color.FromRgb(255,67,67),
            Color.FromRgb(231,72,86),
            Color.FromRgb(232,17,35),
            Color.FromRgb(234,0,94),
            Color.FromRgb(195,0,82),
            Color.FromRgb(227,0,140),
            Color.FromRgb(191,0,119),
            Color.FromRgb(194,57,179),
            Color.FromRgb(154,0,137),
            Color.FromRgb(0,120,212),
            Color.FromRgb(0,99,177),
            Color.FromRgb(142,140,216),
            Color.FromRgb(107,105,214),
            Color.FromRgb(135,100,184),
            Color.FromRgb(116,77,169),
            Color.FromRgb(177,70,194),
            Color.FromRgb(136,23,152),
            Color.FromRgb(0,153,188),
            Color.FromRgb(45,125,154),
            Color.FromRgb(0,183,195),
            Color.FromRgb(3,131,135),
            Color.FromRgb(0,178,148),
            Color.FromRgb(1,133,116),
            Color.FromRgb(0,204,106),
            Color.FromRgb(16,137,62),
            Color.FromRgb(122,117,116),
            Color.FromRgb(93,90,88),
            Color.FromRgb(104,118,138),
            Color.FromRgb(81,92,107),
            Color.FromRgb(86,124,115),
            Color.FromRgb(72,104,96),
            Color.FromRgb(73,130,5),
            Color.FromRgb(16,124,16),
            Color.FromRgb(118,118,118),
            Color.FromRgb(76,74,72),
            Color.FromRgb(105,121,126),
            Color.FromRgb(74,84,89),
            Color.FromRgb(100,124,100),
            Color.FromRgb(82,94,84),
            Color.FromRgb(132,117,69),
            Color.FromRgb(126,115,95)
        };
    }

    private void UpdateAppAccentColor(Color? color)
    {
        Console.WriteLine($"{_faTheme} set the accent color to {color}");
        if (_faTheme != null) _faTheme.CustomAccentColor = color;
    }

    private const string _system = "System";
    private const string _dark = "Dark";
    private const string _light = "Light";
}
