using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ClientApp.Helpers;
using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using FlashCap;

namespace ClientApp.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly CaptureDeviceManager _deviceManager;
    public ObservableCollection<CaptureDeviceDescriptor> DeviceList => _deviceManager.DeviceList;

    [ObservableProperty]
    private ObservableCollection<VideoCharacteristics> _characteristicList = new();
    [ObservableProperty] private VideoCharacteristics? _selectedCharacteristics;
    [ObservableProperty] private CaptureDeviceDescriptor? _selectedDevice;
    [ObservableProperty] private bool _useCustomAccent;

    [ObservableProperty] private Color _customAccentColor;

    [ObservableProperty] private string _currentAppTheme;

    [ObservableProperty] private Color? _listBoxColor;

    private readonly ThemeService _themeService;

    public SettingsViewModel(SettingsService settingsService, ThemeService themeService, CaptureDeviceManager deviceManager)
    {
        _themeService = themeService;
        _deviceManager = deviceManager;
        GetPredefColors();

        CurrentAppTheme = _themeService.Theme;
        UseCustomAccent = _themeService.UseCustomAccent;
        CustomAccentColor = _themeService.Accent;
        SelectedDevice = _deviceManager.SelectedDevice;
        SelectedCharacteristics = _deviceManager.SelectedCharacteristics;

        Console.WriteLine($"Settings View Model : {CurrentAppTheme}, {UseCustomAccent}, {CustomAccentColor}");
    }

    partial void OnSelectedDeviceChanged(CaptureDeviceDescriptor? value)
    {
        CharacteristicList.Clear();
        SelectedCharacteristics = null;

        if (value != null)
        {
            foreach (var characteristic in value.Characteristics)
                CharacteristicList.Add(characteristic);

            SelectedCharacteristics = CharacteristicList.FirstOrDefault();
        }

        _deviceManager.SetSelectedDevice(value);
    }

    partial void OnSelectedCharacteristicsChanged(VideoCharacteristics? value)
    {
        _deviceManager.SetSelectedCharacteristics(value);
    }


    public List<Color> PredefinedColors { get; private set; }

    public string? CurrentVersion =>
        typeof(FluentAvalonia.UI.Controls.NavigationView).Assembly.GetName().Version?.ToString();

    public string CurrentAvaloniaVersion =>
        typeof(Application).Assembly.GetName().Version?.ToString() ?? throw new InvalidOperationException();

    public string[] AppThemes { get; } =
        new[] { System, Light, Dark };


    partial void OnUseCustomAccentChanged(bool value)
    {
        if (value)
        {
            _themeService.SetAccentColor(CustomAccentColor);
        }
        else
        {
            _themeService.DisableCustomAccent();
        }
    }

    partial void OnListBoxColorChanged(Color? value)
    {
        CustomAccentColor = value ?? default;
        _themeService.SetAccentColor(CustomAccentColor);
    }

    partial void OnCustomAccentColorChanged(Color value)
    {
        ListBoxColor = value;
        _themeService.SetAccentColor(value);
    }

    partial void OnCurrentAppThemeChanged(string value)
    {
        _themeService.SetTheme(value);
    }

    private void GetPredefColors()
    {
        PredefinedColors = PredefinedColorHelper.GetPredefinedColors();
    }

    private const string System = "System";
    private const string Dark = "Dark";
    private const string Light = "Light";
}