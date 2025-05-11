using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using ClientApp.Services;
using ClientApp.ViewModels;
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
        var vm = services.GetRequiredService<AuthWindowViewModel>();
        var main = services.GetRequiredService<AuthWindow>();

        main.DataContext = vm;
        main.Show();

        base.OnFrameworkInitializationCompleted();

    }
}