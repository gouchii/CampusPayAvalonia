using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using FluentAvalonia.Styling;

namespace ClientApp.Services;

public class ThemeService
{
    private readonly SettingsService _settingsService;
    private readonly FluentAvaloniaTheme? _faTheme;

    public bool UseCustomAccent { get; private set; }
    public Color Accent { get; private set; }
    public string Theme { get; private set; } = string.Empty;

    public ThemeService(SettingsService settingsService)
    {
        _settingsService = settingsService;
        _faTheme = Application.Current?.Styles[0] as FluentAvaloniaTheme;
        Console.WriteLine($"Theme set to {_faTheme}");
    }

    public void ApplySavedTheme()
    {
        var savedTheme = _settingsService.GetSetting("CurrentAppTheme", "System");
        SetTheme(savedTheme, saveSetting: false);

        var savedUseCustomAccent = _settingsService.GetSetting("UseCustomAccent", false);
        if (savedUseCustomAccent)
        {
            var savedAccent = _settingsService.GetColorSetting("CustomAccentColor", Colors.SlateBlue);
            SetAccentColor(savedAccent, saveSetting: false);
        }
    }

    public void SetTheme(string themeName, bool saveSetting = true)
    {
        if (Theme == themeName) return;

        Theme = themeName;
        var variant = themeName switch
        {
            "Dark" => ThemeVariant.Dark,
            "Light" => ThemeVariant.Light,
            "System" => null,
            _ => null
        };

        if (Application.Current != null) Application.Current.RequestedThemeVariant = variant;

        if (_faTheme != null)
        {
            _faTheme.PreferSystemTheme = (themeName == "System");
        }

        if (saveSetting)
        {
            _settingsService.SetSetting("CurrentAppTheme", themeName);
        }
    }

    public void SetAccentColor(Color color, bool saveSetting = true)
    {
        if (Accent == color) return;

        Accent = color;
        if (_faTheme != null)
        {
            _faTheme.CustomAccentColor = color;
        }

        if (saveSetting)
        {
            _settingsService.SetColorSetting("CustomAccentColor", color);
            _settingsService.SetSetting("UseCustomAccent", true);
            UseCustomAccent = true;
        }
    }

    public void DisableCustomAccent(bool saveSetting = true)
    {
        if (!UseCustomAccent) return;

        UseCustomAccent = false;
        Accent = default;

        if (_faTheme != null)
        {
            _faTheme.CustomAccentColor = null;
        }

        if (saveSetting)
        {
            _settingsService.SetSetting("UseCustomAccent", false);
        }
    }
}
