using Microsoft.Extensions.DependencyInjection;

namespace WinMix.Services;

public class WindowDisplayService : IWindowDisplayService
{
    readonly IServiceProvider _provider;
    readonly IPlaybackService _playback;
    readonly IFileOpenService _fileOpen;
    readonly IStorageService _storage;
    readonly IClipBoardService _clipboard;

 PlayerWindow PlayerWindow => _provider.GetRequiredService<PlayerWindow>();
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

    public void ShowListManager()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            PlayerWindow.Hide();

            if (_listWindow == null)
            {                
                _listWindow = _provider.GetRequiredService<ListManagerWindow>();

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
                _listWindow.Left = PlayerWindow.Left;
                _listWindow.Top = PlayerWindow.Top;
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
            
            PlayerWindow.Show();
            PlayerWindow.Activate();
        });
    }

public string ShowInputWindow()
    {
        var inputDialog = _provider.GetRequiredService<InputDialog>();
        inputDialog.Owner = Application.Current.MainWindow;
inputDialog.ShowDialog();
string result = inputDialog.Response;
        return String.IsNullOrWhiteSpace(result) ? string.Empty : result;
    }

}
