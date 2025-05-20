using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ClientApp.Helpers;
using FlashCap;
using SkiaSharp;

namespace ClientApp.Services;

public class CaptureDeviceManager
{
    private readonly CaptureDevices _captureDevices = new();
    private CaptureDevice? _captureDevice;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly Stopwatch _frameTimer = new();
    private int _frameCount;

    public ObservableCollection<CaptureDeviceDescriptor> DeviceList { get; } = new();
    public CaptureDeviceDescriptor? SelectedDevice { get; set; }
    public ObservableCollection<VideoCharacteristics> CharacteristicsList { get; } = new();
    public VideoCharacteristics? SelectedCharacteristics { get; set; }
    private readonly SettingsService _settingsService;
    public event Action<SKBitmap, int, double, int>? FrameReceived;
    public event Action? CaptureStarted;
    public event Action? CaptureStopped;
    private const int DevicePollIntervalMs = 2000;
    private CancellationTokenSource? _deviceWatcherCts;
    public event Action? DevicesChanged;

    private const string SelectedDeviceSettingKey = "SelectedCameraDeviceName";
    private const string SelectedCharacteristicsSettingKey = "SelectedCameraCharacteristics";

    public CaptureDeviceManager(SettingsService settingsService)
    {
        _settingsService = settingsService;
        StartDeviceWatcher();
    }

    public void StopDeviceWatcher()
    {
        _deviceWatcherCts?.Cancel();
        _deviceWatcherCts?.Dispose();
        _deviceWatcherCts = null;
    }

    private void StartDeviceWatcher()
    {
        _deviceWatcherCts = new CancellationTokenSource();
        Task.Run(async () =>
        {
            while (!_deviceWatcherCts.Token.IsCancellationRequested)
            {
                RefreshDevices();
                await Task.Delay(DevicePollIntervalMs, _deviceWatcherCts.Token);
            }
        }, _deviceWatcherCts.Token);
    }

    public void LoadSelectedDeviceFromSettings()
    {
        if (DeviceList.Count == 0)
        {
            Console.WriteLine("Device list is empty. Skipping device selection.");
            return;
        }
        var rawJson = _settingsService.GetSetting(SelectedDeviceSettingKey, string.Empty);
        Console.WriteLine($"Raw JSON for '{SelectedDeviceSettingKey}': {rawJson}");

        var savedName = _settingsService.GetSetting(SelectedDeviceSettingKey, string.Empty);
        Console.WriteLine($"Trying to load the selected device using saved name: '{savedName}'.");

        SelectedDevice = DeviceList.FirstOrDefault(d => d.Name == savedName);

        if (SelectedDevice == null)
        {
            Console.WriteLine("No matching device found. Falling back to the first available device.");
            SelectedDevice = DeviceList.FirstOrDefault();
        }

        Console.WriteLine($"The loaded selected device is: '{SelectedDevice?.Name}'.");

        if (SelectedDevice == null) return;
        try
        {
            var deviceInfo = _settingsService.GetJson<DeviceInfo>(SelectedCharacteristicsSettingKey);
            if (deviceInfo?.Characteristics == null) return;
            var width = deviceInfo.Characteristics.Width;
            var height = deviceInfo.Characteristics.Height;
            var fps = deviceInfo.Characteristics.FramesPerSecond;
            if (deviceInfo?.DeviceName == SelectedDevice.Name)
            {
                SelectedCharacteristics = SelectedDevice.Characteristics
                    .FirstOrDefault(c => c.Width == width && c.Height == height);

                Console.WriteLine($"Loaded characteristics: {SelectedCharacteristics}");
            }
            else
            {
                Console.WriteLine($"Device name mismatch. Expected '{SelectedDevice.Name}', found '{deviceInfo?.DeviceName}'.");
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Failed to parse saved characteristics: {ex.Message}");
            SelectedCharacteristics = null;
        }
    }


    public void LoadDevices()
    {
        DeviceList.Clear();
        foreach (var descriptor in _captureDevices.EnumerateDescriptors())
        {
            if (descriptor.Characteristics.Length != 0)
            {
                DeviceList.Add(descriptor);
            }
        }

        SelectedDevice = DeviceList.FirstOrDefault();
        SelectedCharacteristics = SelectedDevice?.Characteristics.FirstOrDefault();

        Console.WriteLine($"Found {DeviceList.Count} devices.");
        if (SelectedDevice == null)
            Console.WriteLine("No suitable capture device found.");
    }


    public void SetSelectedCharacteristics(VideoCharacteristics? characteristics)
    {
        Console.WriteLine($"Set Video Characteristics to {characteristics}");
        SelectedCharacteristics = characteristics;

        if (characteristics != null)
        {
            var deviceInfo = new DeviceInfo
            {
                DeviceName = SelectedDevice?.Name,
                Characteristics = new VideoCharacteristicsDto
                {
                    Height = characteristics.Height,
                    Width = characteristics.Width,
                    FramesPerSecond = characteristics.FramesPerSecond
                }
            };

            _settingsService.SetSetting(SelectedCharacteristicsSettingKey, deviceInfo);
        }
        else
        {
            _settingsService.SetSetting(SelectedCharacteristicsSettingKey, string.Empty);
        }
    }


    public void SetSelectedDevice(CaptureDeviceDescriptor? device)
    {
        SelectedDevice = device;

        if (device != null && !string.IsNullOrWhiteSpace(device.Name))
        {
            Console.WriteLine($"Set Device : {device.Name}");
            _settingsService.SetSetting(SelectedDeviceSettingKey, device.Name);
        }
        else
        {
            Console.WriteLine("Clearing selected device setting.");
            _settingsService.SetSetting(SelectedDeviceSettingKey, string.Empty);
        }
    }


    public async Task StartCaptureAsync()
    {
        if (_captureDevice != null)
        {
            Console.WriteLine("Capture already running.");
            return;
        }

        if (SelectedDevice == null || SelectedCharacteristics == null)
        {
            Console.WriteLine("No device or characteristics selected.");
            return;
        }

        if (!DeviceList.Contains(SelectedDevice))
        {
            Console.WriteLine($"Selected device '{SelectedDevice.Name}' is not available.");
            SelectedDevice = null;
            SelectedCharacteristics = null;
            return;
        }

        Console.WriteLine($"Starting capture with device: {SelectedDevice.Name}");
        Console.WriteLine($"Using characteristics: {SelectedCharacteristics.Width}x{SelectedCharacteristics.Height} @ {SelectedCharacteristics.FramesPerSecond}fps");

        _cancellationTokenSource = new CancellationTokenSource();
        _frameTimer.Restart();
        _frameCount = 0;

        try
        {
            _captureDevice = await SelectedDevice.OpenAsync(SelectedCharacteristics, OnFrameReceivedAsync);
            _ = _captureDevice.StartAsync();
            CaptureStarted?.Invoke();
            Console.WriteLine("Camera capture started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start capture: {ex}");
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;

            SelectedDevice = null;
            SelectedCharacteristics = null;
            RefreshDevices();
        }
    }


    public async Task StopCaptureAsync()
    {
        _cancellationTokenSource?.Cancel();

        if (_captureDevice != null)
        {
            try
            {
                await _captureDevice.StopAsync();
                await _captureDevice.DisposeAsync();
                _captureDevice = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping capture: {ex.Message}");
            }
        }

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _frameTimer.Stop();

        CaptureStopped?.Invoke();
    }

    private void OnFrameReceivedAsync(PixelBufferScope bufferScope)
    {
        try
        {
            using var skBitmap = SKBitmap.Decode(bufferScope.Buffer.ReferImage());
            if (skBitmap == null) return;

            const int cropWidth = 400;
            const int cropHeight = 400;
            int x = (skBitmap.Width - cropWidth) / 2;
            int y = (skBitmap.Height - cropHeight) / 2;

            if (x < 0 || y < 0 || cropWidth > skBitmap.Width || cropHeight > skBitmap.Height)
            {
                Console.WriteLine("Crop area is out of bounds.");
                return;
            }

            var croppedBitmap = new SKBitmap(cropWidth, cropHeight);
            using var canvas = new SKCanvas(croppedBitmap);
            var srcRect = new SKRectI(x, y, x + cropWidth, y + cropHeight);
            var destRect = new SKRectI(0, 0, cropWidth, cropHeight);
            canvas.DrawBitmap(skBitmap, srcRect, destRect);

            _frameCount++;
            var elapsedSeconds = _frameTimer.Elapsed.TotalSeconds;
            var fps = elapsedSeconds > 0 ? (_frameCount / elapsedSeconds) : 0;

            FrameReceived?.Invoke(croppedBitmap, _frameCount, fps, croppedBitmap.Width * croppedBitmap.Height);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing frame: {ex.Message}");
        }
        finally
        {
            bufferScope.ReleaseNow();
        }
    }

    public void RefreshDevices()
    {
        var currentDevices = _captureDevices.EnumerateDescriptors().ToList();

        // Find removed devices
        var removedDevices = DeviceList.Where(existing => !currentDevices.Any(d => d.Name == existing.Name)).ToList();
        foreach (var removed in removedDevices)
        {
            Console.WriteLine($"Device '{removed.Name}' removed.");
            DeviceList.Remove(removed);
        }

        // Find new devices
        var newDevices = currentDevices.Where(newDevice => !DeviceList.Any(d => d.Name == newDevice.Name)).ToList();
        foreach (var newDevice in newDevices)
        {
            Console.WriteLine($"Device '{newDevice.Name}' added.");
            DeviceList.Add(newDevice);
        }

        DevicesChanged?.Invoke();
    }
}

public class DeviceInfo
{
    public string? DeviceName { get; init; } = string.Empty;
    public VideoCharacteristicsDto? Characteristics { get; init; }
}

public class VideoCharacteristicsDto
{
    public int Width { get; set; }
    public int Height { get; set; }
    public double FramesPerSecond { get; set; }
}