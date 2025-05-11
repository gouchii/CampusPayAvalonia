using System;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media.Animation;
using FluentAvalonia.UI.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Services;

public class NavigationService
{
    private Frame? _frame;

    private readonly IServiceProvider _serviceProvider;
    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void SetFrame(Frame frame)
    {
        _frame = frame;
    }
    //
    // public void NavigateTo<TView>() where TView : class
    // {
    //     if (_frame == null)
    //         throw new InvalidOperationException("No frame set for navigation.");
    //
    //     _frame.Navigate(typeof(TView));
    // }

    public void NavigateTo<TViewModel>(NavigationTransitionInfo? transitionInfo = null) where TViewModel : class
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        _frame?.NavigateFromObject(viewModel,
            new FluentAvalonia.UI.Navigation.FrameNavigationOptions
            {
                IsNavigationStackEnabled = true,
                TransitionInfoOverride = transitionInfo ?? new SuppressNavigationTransitionInfo()
            });;
    }


}

