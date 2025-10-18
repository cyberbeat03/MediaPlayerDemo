namespace WinMix;

public partial class PlayerWindow : Window
{    
    public PlayerWindow()
    {
        InitializeComponent();
        DataContext = new PlayerViewModel(new PlaybackService());
    }

}
