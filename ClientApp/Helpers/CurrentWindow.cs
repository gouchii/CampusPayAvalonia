using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ClientApp.Helpers;

public static class CurrentWindow
{
    public static Window? Get()
    {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktopLifetime)
            return null;

        // Find the active and visible window
        return desktopLifetime.Windows.FirstOrDefault(window => window.IsActive && window.IsVisible);
    }
}