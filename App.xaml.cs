using WinMix.Services;
using WinMix.ViewModels;

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
        
        var playerViewModel = new PlayerViewModel(playbackService, fileOpenService);
        var listManagerViewModel = new ListManagerViewModel(playbackService, fileOpenService, storageService, clipboardService);

        var window = new PlayerWindow
        {
            DataContext = playerViewModel
        };
        window.Show();
    }
}