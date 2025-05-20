using ClientApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace ClientApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly WindowManagerService _windowManagerService;

    [ObservableProperty] private string _testBox;

    public HomeViewModel(WindowManagerService windowManagerService)
    {
        _windowManagerService = windowManagerService;

    }

    [RelayCommand]
    public void OpenScanDialog()
    {
        _ = _windowManagerService.OpenQrWindowAsDialog();
    }

    [RelayCommand]
    public void OpenCustomerWindow()
    {
        _windowManagerService.OpenCustomerWindow();
    }

    partial void OnTestBoxChanged(string value)
    {

    }





}