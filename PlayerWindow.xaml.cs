namespace WinMix;

public partial class PlayerWindow : Window
{
    PlayerViewModel viewModel = new(new PlaybackService());

    public PlayerWindow()
    {
        InitializeComponent();
        DataContext = viewModel;
    }

}
