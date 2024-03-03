namespace WinMix;

public partial class PlaylistManager : Window
{
    readonly PlaylistViewModel _playVM = new();
    public PlaylistManager()
    {
        InitializeComponent();
        DataContext = _playVM;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
DialogResult     = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
