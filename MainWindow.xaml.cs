using MediaPlayerDemo.ViewModels;

namespace MediaPlayerDemo;

public partial class MainWindow : Window
{
    private MainViewModel _mainVM = new();

    public MainWindow()
    {        
        InitializeComponent();
        DataContext = _mainVM;
    }

    private void AddMedia_Click(object sender, RoutedEventArgs e) => _mainVM.Player.AddMediaFiles();

    private void PlayMedia_Click(object sender, RoutedEventArgs e) => _mainVM.Player.Play();

    private void Pause_Click(object sender, RoutedEventArgs e) => _mainVM.Player.Pause();

    private void NextButton_Click(object sender, RoutedEventArgs e) => _mainVM.Player.PlayNext();

    private void PreviousButton_Click(object sender, RoutedEventArgs e) => _mainVM.Player.PlayPrevious();

    private void RewindButton_Click(object sender, RoutedEventArgs e) => _mainVM.Player.Rewind();

    private void ForwardButton_Click(object sender, RoutedEventArgs e) => _mainVM.Player.FastForward();

    private void CheckBox_Checked(object sender, RoutedEventArgs e) => _mainVM.Player.CanRepeat = true;

    private void CheckBox_Unchecked(object sender, RoutedEventArgs e) => _mainVM.Player.CanRepeat = false;

    private void SpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => _mainVM.Player.ChangeSpeed((double)speedSlider.Value);
    
}
