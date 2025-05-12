using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using FluentAvalonia.UI.Windowing;

namespace ClientApp.Helpers;

public class CustomSplashScreen : IApplicationSplashScreen
{
    public string AppName { get; init; }
    public IImage AppIcon { get; init; }
    public string BackgroundColor { get; init; } = "#202020";
    public object SplashScreenContent { get; }

    public int MinimumShowTime { get; set; } = 1500;


    public CustomSplashScreen()
    {
        var stream = Avalonia.Platform.AssetLoader.Open(new Uri("avares://ClientApp/Assets/avalonia-logo.ico"));
        var bmp = new Bitmap(stream);
        AppIcon = bmp;
        var stackPanel = new StackPanel
        {
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            Spacing = 10,
            Children =
            {
                new Image
                {
                    Source = AppIcon,
                    Width = 128,
                    Height = 128
                },
                new TextBlock
                {
                    Text = AppName,
                    FontSize = 24,
                    Foreground = Brushes.White
                }
            }
        };

        var background = new Border
        {
            Background = new SolidColorBrush(Color.Parse(BackgroundColor)),
            Child = stackPanel
        };

        SplashScreenContent = background;
    }

    public Task RunTasks(CancellationToken cancellationToken)
    {
        // Simulate some loading tasks if needed
        return Task.Delay(MinimumShowTime, cancellationToken);
    }
}