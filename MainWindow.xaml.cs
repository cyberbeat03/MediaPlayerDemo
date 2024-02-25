namespace WinMix;

public partial class MainWindow : Window
{
    readonly MainViewModel _mainVM = new();
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = _mainVM;
    }

    async void Window_Closing(object sender, CancelEventArgs e)
    {
        await _mainVM.CurrentMediaList.SaveAsync("Last Played.wmx");
    }

async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await _mainVM.CurrentMediaList.LoadAsync("Last Played.wmx");
    }

}
