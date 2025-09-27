namespace WinMix;

public partial class PlayerWindow : Window
{
readonly     PlayerViewModel viewModel = new(new PlaybackService());

    public PlayerWindow()
    {
        InitializeComponent();
        DataContext = viewModel;
    }

}
