namespace WinMix;

public partial class ListManagerDialog : Window
{
readonly ListManagerViewModel _playVM;

    public PlaybackList Playlist { get; private set; }

    public ListManagerDialog(PlaybackList mediaList)
    {                
        InitializeComponent();

        Playlist = mediaList;
        _playVM = new ListManagerViewModel(Playlist.Items);
        DataContext = _playVM;
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Playlist = _playVM.GetPlaybackList();
DialogResult     = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
