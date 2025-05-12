using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Services;

public class NavigationService
{
    private readonly NavigationPageFactory _navigationPageFactory;
    private readonly Dictionary<Window, Dictionary<string, Frame>> _windowFrames = new();
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider, NavigationPageFactory navigationPageFactory)
    {
        _serviceProvider = serviceProvider;
        _navigationPageFactory = navigationPageFactory;
    }

    public void RegisterFrame(Window window, Frame frame, string? frameName = null)
    {
        if (!_windowFrames.TryGetValue(window, out var frames))
        {
            frames = new Dictionary<string, Frame>();
            _windowFrames[window] = frames;
            window.Closed += (_, _) => UnregisterAllFrames(window);
        }

        frameName ??= $"Frame_{Guid.NewGuid()}";
        frame.NavigationPageFactory = _navigationPageFactory;
        frames[frameName] = frame;
        Console.WriteLine($"Registered frame: {frameName} in window: {window.Title}");
    }

    public void UnregisterAllFrames(Window window)
    {
        if (_windowFrames.TryGetValue(window, out var frames))
        {
            foreach (var (name, frame) in frames)
            {
                frame.BackStack.Clear();
                frame.ForwardStack.Clear();
                frame.Content = null;
                Console.WriteLine($"Unregistering frame: {name} from window: {window.Title}");
            }
            _windowFrames.Remove(window);
        }
    }

    public void UnregisterFrame(Window window, string frameName)
    {
        if (_windowFrames.TryGetValue(window, out var frames) && frames.Remove(frameName, out var frame))
        {
            frame.BackStack.Clear();
            frame.ForwardStack.Clear();
            frame.Content = null;
        }
    }

    public void NavigateTo<TViewModel>(Window window, string frameName, NavigationTransitionInfo? transitionInfo = null) where TViewModel : class
    {
        if (!_windowFrames.TryGetValue(window, out var frames) || !frames.TryGetValue(frameName, out var frame))
            throw new InvalidOperationException($"No frame registered with the name '{frameName}' for the given window.");

        var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<TViewModel>(_serviceProvider);
        Console.WriteLine($"Navigating to {viewModel}, with window : {window} and frame {frameName}");
        frame.NavigateFromObject(viewModel, new FluentAvalonia.UI.Navigation.FrameNavigationOptions
        {
            IsNavigationStackEnabled = true,
            TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
        });
    }

    public void NavigateTo<TViewModel>(Frame frame, NavigationTransitionInfo? transitionInfo = null) where TViewModel : class
    {
        var viewModel = ActivatorUtilities.GetServiceOrCreateInstance<TViewModel>(_serviceProvider);
        frame.NavigateFromObject(viewModel, new FluentAvalonia.UI.Navigation.FrameNavigationOptions
        {
            IsNavigationStackEnabled = true,
            TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
        });
    }
}
