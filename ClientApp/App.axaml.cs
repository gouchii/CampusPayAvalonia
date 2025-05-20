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
        var deviceManager = services.GetService<CaptureDeviceManager>();
        preferences?.LoadSettings();
        deviceManager?.LoadDevices();
        deviceManager?.LoadSelectedDeviceFromSettings();
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