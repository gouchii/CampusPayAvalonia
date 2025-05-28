using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp.Rendering;

namespace ClientApp.ViewModels;

public partial class QrGeneratorWindowViewModel : ViewModelBase
{
    [ObservableProperty] private SKBitmap? _generatedQrCode;

    [ObservableProperty]
    private string? _transactionRef;

    partial void OnTransactionRefChanged(string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            GeneratedQrCode = GenerateQrCode(value);
        }
    }

    private SKBitmap GenerateQrCode(string text, int width = 400, int height = 400)
    {
        var writer = new BarcodeWriter<SKBitmap>
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new EncodingOptions
            {
                Height = height,
                Width = width,
                Margin = 1
            },
            Renderer = new SKBitmapRenderer()
        };

        return writer.Write(text);
    }
}