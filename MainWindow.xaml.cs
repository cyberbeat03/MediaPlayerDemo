namespace WinMix;

public partial class MainWindow : Window
{        
    public MainWindow()
    {        
        DataContext = new PlayerViewModel();
        InitializeComponent();
        StatusText.Focus();
    }

}
