namespace WinMix;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {        
        base.OnStartup(e);

        IPlaybackService playbackService = new PlaybackService();
        PlayerViewModel viewModel = new(playbackService);

        PlayerWindow window = new(viewModel);
            window.Show();
        }
    }
