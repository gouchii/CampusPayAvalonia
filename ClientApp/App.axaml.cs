using System;
using Avalonia;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using ClientApp.Services;
using ClientApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

    }

    public override void OnFrameworkInitializationCompleted()
    {
        var collection = new ServiceCollection();
        collection.AddCommonServices();

        var services = collection.BuildServiceProvider();
        var windowManager = services.GetRequiredService<WindowManagerService>();
        var preferences = services.GetService<SettingsService>();
        var themeService = services.GetService<ThemeService>();
        var deviceManager = services.GetService<DeviceManager>();
        _=deviceManager?.InitializeAsync();
        if (deviceManager != null)

            deviceManager.DevicesChanged += (s, devices) =>
            {
                Console.WriteLine($"Devices changed event triggered. Total devices: {devices.Count}");
                foreach (var d in devices)
                {
                    Console.WriteLine($"- {d.ProductName} (VID:{d.VendorId:X4} PID:{d.ProductId:X4})");
                }
            };
        preferences?.LoadSettings();
        themeService?.ApplySavedTheme();
        DisableAvaloniaDataAnnotationValidation();

        windowManager.OpenMainWindowAuthAsDialog();

        base.OnFrameworkInitializationCompleted();

    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}