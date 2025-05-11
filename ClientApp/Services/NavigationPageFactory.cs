using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace ClientApp.Services;

public class NavigationPageFactory : INavigationPageFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationPageFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Control GetPage(Type srcType)
    {var view = _serviceProvider.GetService(srcType);
        if (view is Control control)
            return control;

        throw new InvalidOperationException($"Unknown page type: {srcType}");
    }

    public Control GetPageFromObject(object target)
    {
        var targetType = target.GetType();
        var viewTypeName = targetType.AssemblyQualifiedName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var viewType = Type.GetType(viewTypeName);

        if (viewType == null)
            throw new InvalidOperationException($"Could not find view type for {targetType.Name}");

        var view = _serviceProvider.GetService(viewType);
        if (view is Control control)
            return control;

        throw new InvalidOperationException($"No registered service for {viewType.FullName}");
    }

}