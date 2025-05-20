using System;
using System.Threading.Tasks;
using ClientApp.Services;
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
    private readonly CaptureDeviceManager _deviceManager;
    private readonly WindowManagerService _windowManagerService;

    [ObservableProperty] private SKBitmap? _cameraFrame;
    [ObservableProperty] private string _frameRate = "Frame Rate: 0 fps";
    [ObservableProperty] private string _frameResolution = "Resolution: N/A";
    [ObservableProperty] private string _frameCount = "Frames Captured: 0";
    [ObservableProperty] private string _qrCodeText = "No QR code detected";

    private int _decodeCounter;
    private const int DecodeSkipFrames = 5;

    public QrScannerWindowViewModel(CaptureDeviceManager deviceManager, WindowManagerService windowManagerService)
    {
        _deviceManager = deviceManager;
        _windowManagerService = windowManagerService;

        _deviceManager.FrameReceived += OnFrameReceived;
        _deviceManager.CaptureStarted += OnCaptureStarted;
        _deviceManager.CaptureStopped += OnCaptureStopped;

        _ = _deviceManager.StartCaptureAsync();
    }

    public void OnClosed()
    {
        _ = _deviceManager.StopCaptureAsync();
        Console.WriteLine("Camera stopped on window close.");
    }

    private void OnCaptureStarted()
    {
        Console.WriteLine("Camera capture started.");
        FrameRate = "Frame Rate: 0 fps";
        FrameCount = "Frames Captured: 0";
        FrameResolution = "Resolution: N/A";
        CameraFrame = null;
    }

    private void OnCaptureStopped()
    {
        Console.WriteLine("Camera capture stopped.");
        FrameRate = "Frame Rate: 0 fps";
        FrameCount = "Frames Captured: 0";
        FrameResolution = "Resolution: N/A";
        CameraFrame = null;
    }

    private void OnFrameReceived(SKBitmap frame, int frameCount, double fps, int pixelCount)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            CameraFrame = frame;
            FrameResolution = $"Resolution: {frame.Width}x{frame.Height}";
            FrameCount = $"Frames Captured: {frameCount}";
            FrameRate = $"Frame Rate: {fps:F2} fps";
        });

        if (_decodeCounter++ % DecodeSkipFrames == 0)
        {
            Task.Run(() =>
            {
                var reader = new BarcodeReader
                {
                    AutoRotate = true,
                    Options = new DecodingOptions
                    {
                        TryHarder = true,
                        PossibleFormats = new[] { BarcodeFormat.QR_CODE }
                    }
                };

                var result = reader.Decode(frame);
                if (result != null)
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        QrCodeText = $"QR Code: {result.Text}";
                        Console.WriteLine($"QR Code detected: {result.Text}");

                        _ = _deviceManager.StopCaptureAsync();
                        _windowManagerService.CloseWindow("QrWindow");
                    });
                }
                else
                {
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => { QrCodeText = "No QR code detected"; });
                }
            });
        }
    }

    [RelayCommand]
    private async Task StopCaptureAsync()
    {
        await _deviceManager.StopCaptureAsync();
    }
}
