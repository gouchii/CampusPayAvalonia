using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Services;

public class NavigationService
{
    private readonly NavigationPageFactory _navigationPageFactory;
    private readonly Dictionary<string, Frame> _frames = new();

    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider, NavigationPageFactory navigationPageFactory)
    {
        _serviceProvider = serviceProvider;
        _navigationPageFactory = navigationPageFactory;
    }

    public void RegisterFrame(string frameName, Frame frame)
    {
        frame.NavigationPageFactory = _navigationPageFactory;
        _frames[frameName] = frame;
    }

    public void UnregisterFrame(string frameName)
    {
        if (_frames.Remove(frameName, out var frame))
        {
            // Optionally clear the frame's navigation history to free up memory
            frame.BackStack.Clear();
            frame.ForwardStack.Clear();
            frame.Content = null;  // Detach any current content
        }
    }

    public void NavigateTo<TViewModel>(string frameName, NavigationTransitionInfo? transitionInfo = null) where TViewModel : class
    {
        if (!_frames.TryGetValue(frameName, out var frame))
            throw new InvalidOperationException($"No frame registered with the name '{frameName}'");

        var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<TViewModel>(_serviceProvider);
        frame.NavigateFromObject(viewModel, new FluentAvalonia.UI.Navigation.FrameNavigationOptions
        {
            IsNavigationStackEnabled = true,
            TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
        });

    }
}