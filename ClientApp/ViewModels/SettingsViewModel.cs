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

    public SettingsViewModel(ThemeService themeService, CaptureDeviceManager deviceManager)
    {
        _themeService = themeService;
        _deviceManager = deviceManager;
        GetPredefColors();

        CurrentAppTheme = _themeService.Theme;
        UseCustomAccent = _themeService.UseCustomAccent;
        CustomAccentColor = _themeService.Accent;

        SelectedDevice = _deviceManager.SelectedDevice;

        if (SelectedDevice != null)
        {
            CharacteristicList.Clear();
            foreach (var characteristic in SelectedDevice.Characteristics)
            {
                CharacteristicList.Add(characteristic);
            }

            if (_deviceManager.SelectedCharacteristics != null)
            {
                SelectedCharacteristics = CharacteristicList.FirstOrDefault(c =>
                    Math.Abs(c.FramesPerSecond - _deviceManager.SelectedCharacteristics.FramesPerSecond) < 0.01 &&
                    c.Width == _deviceManager.SelectedCharacteristics.Width &&
                    c.Height == _deviceManager.SelectedCharacteristics.Height);
            }
            else
            {
                SelectedCharacteristics = CharacteristicList.FirstOrDefault();
            }
        }
        SelectedCharacteristics = _deviceManager.SelectedCharacteristics;
        _deviceManager.DevicesChanged += OnDevicesChanged;
        Console.WriteLine($"Selected Cam : {SelectedDevice?.Name}");
        Console.WriteLine($"Settings View Model : {CurrentAppTheme}, {UseCustomAccent}, {CustomAccentColor}");
    }



    private bool _isUpdatingDevices;

    private void OnDevicesChanged()
    {
        if (_isUpdatingDevices) return;

        _isUpdatingDevices = true;
        try
        {
            if (SelectedDevice != null && !_deviceManager.DeviceList.Contains(SelectedDevice))
            {
                Console.WriteLine($"Selected device '{SelectedDevice.Name}' is no longer available.");
                SelectedDevice = _deviceManager.DeviceList.FirstOrDefault();
                SelectedCharacteristics = null;
            }

            if (SelectedDevice == null || CharacteristicList.Count != 0) return;
            CharacteristicList.Clear();
            foreach (var characteristic in SelectedDevice.Characteristics)
            {
                CharacteristicList.Add(characteristic);
            }

            if (SelectedCharacteristics == null)
            {
                SelectedCharacteristics = CharacteristicList.FirstOrDefault();
            }
        }
        finally
        {
            _isUpdatingDevices = false;
        }
    }



    partial void OnSelectedDeviceChanged(CaptureDeviceDescriptor? value)
    {
        if (_deviceManager.SelectedDevice == value)
            return;

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
        if (_deviceManager.SelectedCharacteristics != null && _deviceManager.SelectedCharacteristics.Equals(value))
            return;

        _deviceManager.SetSelectedCharacteristics(value);
    }


    public List<Color>? PredefinedColors { get; private set; }

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