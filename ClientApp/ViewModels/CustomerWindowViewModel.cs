using ClientApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels;

public partial class CustomerWindowViewModel : ViewModelBase
{
    private readonly WindowManagerService _windowManagerService;

    public CustomerWindowViewModel(WindowManagerService windowManagerService)
    {
        _windowManagerService = windowManagerService;
    }

    [RelayCommand]
    public void OpenQrWindow()
    {
        _windowManagerService. OpenAuthWindowAsDialogBase("CustomerWindow");
    }
}