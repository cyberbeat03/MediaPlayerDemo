namespace WinMix;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {        
        base.OnStartup(e);
        
        var playbackService = new PlaybackService();
        var fileOpenService = new FileOpenService();
        var storageService = new StorageService();
        var clipboardService = new ClipBoardService();
        
        // create the player window first (no DataContext yet)
        var window = new PlayerWindow();

        // create the window display service which needs the PlayerWindow
        var windowDisplayService = new WindowDisplayService(window, playbackService, fileOpenService, storageService, clipboardService);

        // create the player viewmodel with the window display service injected
        var playerViewModel = new PlayerViewModel(playbackService, fileOpenService, windowDisplayService);
        window.DataContext = playerViewModel;

        window.Show();
    }
}