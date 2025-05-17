using System;
using ClientApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly WindowManagerService _windowManagerService;

    public HomeViewModel(IServiceProvider serviceProvider, WindowManagerService windowManagerService)
    {
        _serviceProvider = serviceProvider;
        _windowManagerService = windowManagerService;
    }

    public HomeViewModel()
    {
        throw new NotImplementedException();
    }

    [RelayCommand]
    public void OpenScanDialog()
    {
        _ = _windowManagerService.OpenQrWindowAsDialog();
    }
}



