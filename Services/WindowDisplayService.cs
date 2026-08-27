using Microsoft.Extensions.DependencyInjection;

namespace WinMix.Services;

public class WindowDisplayService : IWindowDisplayService
{
    readonly IServiceProvider _provider;
    readonly IPlaybackService _playback;
    readonly IFileOpenService _fileOpen;
    readonly IStorageService _storage;
    readonly IClipBoardService _clipboard;

    ListManagerWindow? _listWindow;

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

    // Lazily resolve the PlayerWindow to avoid a constructor cycle
    PlayerWindow PlayerWindowInstance => _provider.GetRequiredService<PlayerWindow>();

    public void ShowListManager()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            PlayerWindowInstance.Hide();

            if (_listWindow == null)
            {
                var listVm = new ListManagerViewModel(_playback, _fileOpen, _storage, _clipboard, this);
                _listWindow = new ListManagerWindow
                {
                    DataContext = listVm
                };

                _listWindow.Closed += (s, e) =>
                {
                    _listWindow = null;
                    ShowPlayer();
                };
            }

            // Match the list manager window location to the player window's current position
            try
            {
                _listWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
                _listWindow.Left = PlayerWindowInstance.Left;
                _listWindow.Top = PlayerWindowInstance.Top;
            }
            catch
            {
                // ignore any issue setting placement
            }

            _listWindow.Show();
            _listWindow.Activate();
        });
    }

    public void ShowPlayer()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (_listWindow != null)
            {
                try { _listWindow.Close(); } catch { }
                _listWindow = null;
            }
            
            PlayerWindowInstance.Show();
            PlayerWindowInstance.Activate();
        });
    }
}
