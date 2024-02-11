namespace MediaPlayerDemo;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainVM = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _mainVM;
    }            
    
private void CheckBox_Checked(object sender, RoutedEventArgs e) => _mainVM.CanRepeat = true;

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e) => _mainVM.CanRepeat = false;

    private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => _mainVM.ChangeSpeed((double)speedSlider.Value);
}
