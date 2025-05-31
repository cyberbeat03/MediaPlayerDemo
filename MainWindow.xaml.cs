namespace WinMix;

public partial class MainWindow : Window
{
    PlayerViewModel viewmodel = new PlayerViewModel(new PlaybackList());

    public MainWindow()
    {        
        InitializeComponent();
        DataContext = viewmodel;
        StatusText.Focus();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {

    }
}
