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
        using (var stream = AssetLoader.Open(new Uri(name)))
        using (StreamReader reader = new StreamReader(stream))
        {
            return reader.ReadToEnd();
        }
    }

    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            SetProperty(ref field, value, propertyName);
            return true;
        }
        return false;
    }

    [ObservableProperty]
    private string navHeader;

    [ObservableProperty]
    private string iconKey;

    [ObservableProperty]
    private bool showsInFooter;

    public partial class MainPageViewModelBase : ViewModelBase
    {
    }
}