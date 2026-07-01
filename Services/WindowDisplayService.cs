namespace WinMix.Services;

public class WindowDisplayService : IWindowDisplayService
{
    readonly PlayerWindow _playerWindow;
    readonly IPlaybackService _playback;
    readonly IFileOpenService _fileOpen;
    readonly IStorageService _storage;
    readonly IClipBoardService _clipboard;

    ListManagerWindow? _listWindow;

    public WindowDisplayService(PlayerWindow playerWindow,
        IPlaybackService playback,
        IFileOpenService fileOpen,
        IStorageService storage,
        IClipBoardService clipboard)
    {
        _playerWindow = playerWindow ?? throw new ArgumentNullException(nameof(playerWindow));
        _playback = playback ?? throw new ArgumentNullException(nameof(playback));
        _fileOpen = fileOpen ?? throw new ArgumentNullException(nameof(fileOpen));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        _clipboard = clipboard ?? throw new ArgumentNullException(nameof(clipboard));
    }

    public void ShowListManager()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _playerWindow.Hide();

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

            _playerWindow.Show();
            _playerWindow.Activate();
        });
    }
}
