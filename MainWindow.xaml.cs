namespace WinMix;

public partial class MainWindow : Window
{    
    public MainWindow()
    {        
        InitializeComponent();
        DataContext = new PlayerViewModel(new PlaybackList());
        ItemStatus.Focus();
    }
}
