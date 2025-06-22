namespace WinMix;

public partial class MainWindow : Window
{    
    public MainWindow()
    {        
        InitializeComponent();
        DataContext = new PlayerViewModel(new PlaybackList());
MediaItemsList        .Focus();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {

    }
}
