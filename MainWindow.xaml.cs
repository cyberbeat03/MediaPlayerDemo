namespace WinMix;

public partial class MainWindow : Window
{
    readonly MainViewModel _mainVM = new();
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = _mainVM;
    }

    void Window_Closing(object sender, CancelEventArgs e)
    {
        
    }

 void Window_Loaded(object sender, RoutedEventArgs e)
    {

    }

}
