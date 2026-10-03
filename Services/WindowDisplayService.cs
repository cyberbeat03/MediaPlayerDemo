using Microsoft.Extensions.DependencyInjection;
using WinMix.Interfaces;

namespace WinMix.Services;

public class WindowDisplayService : IWindowDisplayService
{
    readonly IServiceProvider _provider;

    public WindowDisplayService(IServiceProvider provider)        
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));       
    }

    PlayerWindow GetPlayerWindow() => _provider.GetRequiredService<PlayerWindow>();

    public Task<string?> PickPlaylistFileAsync()
    {        
        var op = Application.Current.Dispatcher.InvokeAsync(() =>
        {
            var wnd = _provider.GetRequiredService<ListManagerWindow>();            
            wnd.Owner = Application.Current?.MainWindow;
            return wnd.ShowDialog() == true ? wnd.GetSelectedPlaylistPath() : null;
        });
        return op.Task;
    }

    public string ShowInputDialog()
    {
        var inputDialog = _provider.GetRequiredService<InputDialog>();
        inputDialog.Owner = Application.Current?.MainWindow;
        return inputDialog.ShowDialog() == true
            ? (string.IsNullOrWhiteSpace(inputDialog.Response) ? string.Empty : inputDialog.Response)
            : string.Empty;
    }

    public void ShowAboutDialog()
    {
        var aboutDialog = _provider.GetRequiredService<AboutDialog>();
        aboutDialog.Owner = Application.Current?.MainWindow;
        aboutDialog.ShowDialog();
    }
}
