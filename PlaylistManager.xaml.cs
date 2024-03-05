namespace WinMix;

public partial class PlaylistManager : Window
{
readonly PlaylistViewModel _playVM;

    public PlaybackList Playlist { get; private set; }

    public PlaylistManager(PlaybackList mediaList)
    {                
        InitializeComponent();

        Playlist = mediaList;
        _playVM = new PlaylistViewModel(Playlist.Items);
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
