namespace WinMix;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {        
        base.OnStartup(e);

        var playbackService = new PlaybackService();
        var viewModel = new PlayerViewModel(playbackService);
        PlayerWindow window = new(viewModel);
            window.Show();
        }
    }
