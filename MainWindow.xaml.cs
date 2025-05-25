namespace WinMix;

public partial class MainWindow : Window
{        
    public MainWindow()
    {        
        DataContext = new PlayerViewModel();
        InitializeComponent();
        StatusText.Focus();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {

    }
}
