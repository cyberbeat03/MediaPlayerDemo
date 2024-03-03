namespace WinMix;

public partial class PlaylistManager : Window
{
    PlaylistViewModel _playVM;

    public PlaybackList Playlist { get; set; }

    public PlaylistManager(PlaybackList mediaList)
    {
        Playlist = mediaList;
        _playVM = new PlaylistViewModel(Playlist.Items);
        
        InitializeComponent();
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
