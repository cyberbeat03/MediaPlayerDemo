namespace MediaPlayerDemo;

public partial class MainWindow : Window
{
    private readonly MainViewModel _mainVM = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _mainVM;
    }

    private void AddMedia_Click(object sender, RoutedEventArgs e) => _mainVM.AddMediaFiles();

    private void PlayMedia_Click(object sender, RoutedEventArgs e) => _mainVM.Play();

    private void Pause_Click(object sender, RoutedEventArgs e) => _mainVM.Pause();

    private void NextButton_Click(object sender, RoutedEventArgs e) => _mainVM.PlayNext();

    private void PreviousButton_Click(object sender, RoutedEventArgs e) => _mainVM.PlayPrevious();

    private void RewindButton_Click(object sender, RoutedEventArgs e) => _mainVM.Rewind();

    private void ForwardButton_Click(object sender, RoutedEventArgs e) => _mainVM.FastForward();

    private void CheckBox_Checked(object sender, RoutedEventArgs e) => _mainVM.CanRepeat = true;

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e) => _mainVM.CanRepeat = false;

    private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => _mainVM.ChangeSpeed((double)speedSlider.Value);

}
