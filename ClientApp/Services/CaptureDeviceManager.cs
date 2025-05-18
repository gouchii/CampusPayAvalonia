using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

    private const string SelectedDeviceSettingKey = "SelectedCameraDeviceName";
    private const string SelectedCharacteristicsSettingKey = "SelectedCameraCharacteristics";

    public CaptureDeviceManager(SettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    public void LoadSelectedDeviceFromSettings()
    {
        var savedName = _settingsService.GetSetting(SelectedDeviceSettingKey, string.Empty);
        if (!string.IsNullOrEmpty(savedName))
        {
            SelectedDevice = DeviceList.FirstOrDefault(d => d.Name == savedName);
        }
        else
        {
            SelectedDevice = DeviceList.FirstOrDefault();
        }

        // Load the selected characteristics if a device is found
        if (SelectedDevice != null)
        {
            try
            {
                var json = _settingsService.GetSetting(SelectedCharacteristicsSettingKey, string.Empty);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var deviceInfo = JsonSerializer.Deserialize<dynamic>(json);
                    var width = (int)(deviceInfo?.Characteristics.Width ?? throw new InvalidOperationException());
                    var height = (int)deviceInfo.Characteristics.Height;
                    var fps = (double)deviceInfo.Characteristics.FramesPerSecond;

                    SelectedCharacteristics = SelectedDevice.Characteristics
                        .FirstOrDefault(c => c.Width == width && c.Height == height && c.FramesPerSecond == fps);
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Failed to parse saved characteristics: {ex.Message}");
            }
        }
    }



    public void LoadDevices()
    {
        DeviceList.Clear();
        foreach (var descriptor in _captureDevices.EnumerateDescriptors())
        {
            if (descriptor.Characteristics.Any())
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
        SelectedCharacteristics = characteristics;

        if (characteristics != null)
        {
            // Save both the device name and the selected characteristics
            var deviceInfo = new
            {
                DeviceName = SelectedDevice?.Name,
                Characteristics = new
                {
                    characteristics.Width,
                    characteristics.Height,
                    characteristics.FramesPerSecond
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

        if (device != null)
            _settingsService.SetSetting(SelectedDeviceSettingKey, device);
        else
            _settingsService.SetSetting(SelectedDeviceSettingKey, string.Empty);
    }

    // private void LoadSelectedDeviceFromSettings()
    // {
    //     var savedName = _settingsService.GetSetting(SelectedDeviceSettingKey, string.Empty);
    //     if (!string.IsNullOrEmpty(savedName))
    //     {
    //         SelectedDevice = DeviceList.FirstOrDefault(d => d.Name == savedName);
    //     }
    //     else
    //     {
    //         SelectedDevice = DeviceList.FirstOrDefault();
    //     }
    // }

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

        // Check if the selected device is still connected
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

            // Clear the selected device and characteristics on failure
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

            // Remove missing devices
            for (int i = DeviceList.Count - 1; i >= 0; i--)
            {
                var device = DeviceList[i];
                if (!currentDevices.Any(d => d.Name == device.Name))
                {
                    Console.WriteLine($"Device '{device.Name}' removed.");
                    DeviceList.RemoveAt(i);

                    // Clear selection if the removed device was selected
                    if (SelectedDevice == device)
                    {
                        SelectedDevice = null;
                        SelectedCharacteristics = null;
                    }
                }
            }

            // Add new devices
            foreach (var newDevice in currentDevices)
            {
                if (!DeviceList.Any(d => d.Name == newDevice.Name))
                {
                    Console.WriteLine($"Device '{newDevice.Name}' added.");
                    DeviceList.Add(newDevice);
                }
            }

            // Re-select the last known device if still available
            if (SelectedDevice == null && DeviceList.Count > 0)
            {
                LoadSelectedDeviceFromSettings();
            }

            Console.WriteLine($"Device list refreshed. {DeviceList.Count} devices available.");
        }

    }
