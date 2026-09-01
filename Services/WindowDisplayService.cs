using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WinMix.Services;

public class WindowDisplayService : IWindowDisplayService
{
    readonly IServiceProvider _provider;
    readonly IPlaybackService _playback;
    readonly IFileOpenService _fileOpen;
    readonly IStorageService _storage;
    readonly IClipBoardService _clipboard;

    public WindowDisplayService(IServiceProvider provider,
        IPlaybackService playback,
        IFileOpenService fileOpen,
        IStorageService storage,
        IClipBoardService clipboard)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _playback = playback ?? throw new ArgumentNullException(nameof(playback));
        _fileOpen = fileOpen ?? throw new ArgumentNullException(nameof(fileOpen));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
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
