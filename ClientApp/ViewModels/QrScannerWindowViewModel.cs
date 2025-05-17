using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlashCap;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

namespace ClientApp.ViewModels;

public partial class QrScannerWindowViewModel : ObservableObject
{
    private readonly CaptureDevices _captureDevices = new();
    private CaptureDevice? _captureDevice;
    private CancellationTokenSource? _cancellationTokenSource;
    private Stopwatch _frameTimer = new();
    private int _nFrameCount;

    [ObservableProperty]
    private SKBitmap? _cameraFrame;

    [ObservableProperty]
    private ObservableCollection<CaptureDeviceDescriptor> _deviceList = new();

    [ObservableProperty]
    private CaptureDeviceDescriptor? _selectedDevice;

    [ObservableProperty]
    private ObservableCollection<VideoCharacteristics> _characteristicsList = new();

    [ObservableProperty]
    private VideoCharacteristics? _selectedCharacteristics;

    [ObservableProperty]
    private string _frameRate = "Frame Rate: 0 fps";

    [ObservableProperty]
    private string _frameResolution = "Resolution: N/A";

    [ObservableProperty]
    private string _frameCount = "Frames Captured: 0";

    [ObservableProperty]
    private string _qrCodeText = "No QR code detected";

    public QrScannerWindowViewModel()
    {
        LoadDevices();
    }

    private void LoadDevices()
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

    partial void OnSelectedDeviceChanged(CaptureDeviceDescriptor? value)
    {
        // Automatically update characteristics when device changes
        CharacteristicsList.Clear();
        if (value != null)
        {
            foreach (var characteristic in value.Characteristics)
            {
                CharacteristicsList.Add(characteristic);
            }
            SelectedCharacteristics = CharacteristicsList.FirstOrDefault();
        }
        else
        {
            SelectedCharacteristics = null;
        }
    }

    [RelayCommand]
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

        Console.WriteLine($"Starting capture with device: {SelectedDevice.Name}");
        Console.WriteLine($"Using characteristics: {SelectedCharacteristics.Width}x{SelectedCharacteristics.Height} @ {SelectedCharacteristics.FramesPerSecond}fps");

        _cancellationTokenSource = new CancellationTokenSource();
        _frameTimer.Restart();
        _nFrameCount = 0;
        FrameCount = "Frames Captured: 0";
        FrameRate = "Frame Rate: 0 fps";
        FrameResolution = "Resolution: N/A";

        try
        {
            _captureDevice = await SelectedDevice.OpenAsync(SelectedCharacteristics, OnFrameReceivedAsync);
            _ = _captureDevice.StartAsync();
            Console.WriteLine("Camera capture started.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start capture: {ex}");
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }


    [RelayCommand]
    public async Task StopCaptureAsync()
    {
        _cancellationTokenSource?.Cancel();

        if (_captureDevice != null)
        {
            try
            {
                await _captureDevice.StopAsync();
                await _captureDevice.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping capture: {ex.Message}");
            }
            finally
            {
                _captureDevice = null;
            }
        }

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        _frameTimer.Stop();

        FrameRate = "Frame Rate: 0 fps";
        FrameCount = "Frames Captured: 0";
        FrameResolution = "Resolution: N/A";
        CameraFrame = null;
    }

    private void OnFrameReceivedAsync(PixelBufferScope bufferScope)
    {
        try
        {
            var imageBytes = bufferScope.Buffer.ReferImage();
            // await using var ms = new MemoryStream(imageBytes);
            // var bitmap = new Bitmap(ms);

            var skBitmap = SKBitmap.Decode(imageBytes);
            // Decode QR code
            var reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                }
            };

            var result = reader.Decode(skBitmap);
            if (result != null)
            {
                QrCodeText = $"QR Code: {result.Text}";
                Console.WriteLine($"QR Code detected: {result.Text}");
            }
            else
            {
                QrCodeText = "No QR code detected";
            }

            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                CameraFrame = skBitmap;
                FrameResolution = $"Resolution: {skBitmap.Width}x{skBitmap.Width}";
                _nFrameCount++;
                FrameCount = $"Frames Captured: {_nFrameCount}";

                var elapsedSeconds = _frameTimer.Elapsed.TotalSeconds;
                if (elapsedSeconds > 0)
                {
                    FrameRate = $"Frame Rate: {(_nFrameCount / elapsedSeconds):F2} fps";
                }
            });
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
}



