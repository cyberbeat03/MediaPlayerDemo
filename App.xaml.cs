namespace WinMix;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {        
        base.OnStartup(e);

        var playbackService = new PlaybackService();
        var viewModel = new PlayerViewModel(playbackService);

        var window = new PlayerWindow
        {
            DataContext = viewModel
        };
        window.Show();
        }
    }
