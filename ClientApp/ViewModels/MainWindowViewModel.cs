using ClientApp.Services;

namespace ClientApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{

    private readonly WindowManagerService _windowManagerService;
    private readonly TokenManager _tokenManager;

    public MainWindowViewModel(TokenManager tokenManager, WindowManagerService windowManagerService)
    {
        _tokenManager = tokenManager;
        _windowManagerService = windowManagerService;
        _tokenManager.OnTimeout += HandleInactivityTimeout;
    }

    private void HandleInactivityTimeout()
    {
        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
        {
            await _windowManagerService.OpenAuthWindowAsDialogBaseAsync("MainWindow");
        });
    }



}