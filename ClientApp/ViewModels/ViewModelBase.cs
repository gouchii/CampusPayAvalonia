using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientApp.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    protected string GetAssemblyResource(string name)
    {
        using var stream = AssetLoader.Open(new Uri(name));
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    protected new bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            SetProperty(ref field, value, propertyName);
            return true;
        }
        return false;
    }

    [ObservableProperty]
    private string _navHeader;

    [ObservableProperty]
    private string _iconKey;

    [ObservableProperty]
    private bool _showsInFooter;

    public partial class MainPageViewModelBase : ViewModelBase
    {
    }
}