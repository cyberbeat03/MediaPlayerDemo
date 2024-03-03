using WinMix.Services;

namespace WinMix.ViewModels;

public partial class MainViewModel : MainViewModelBase
{
    private DispatcherTimer _timer = new();

    public MainViewModel()
    {        
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.MediaUri))
        {
            MPlayer.Source = currentItem.MediaUri;
            Play();
            GetMediaStatus();
        }
    }

    private void Media_Opened(object sender, RoutedEventArgs e)
    {
        TotalDuration = MPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        _timer.Start();
    }

    private void Media_Ended(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = "00:00";
        if (CanRepeat)
            Play();
        else
            Next();
    }

    [RelayCommand]
    void Play()
    {
        if (MPlayer.Source is not null)
        {
            _timer.IsEnabled = true;
            MPlayer.Play();
        }
    }

    [RelayCommand]
    void Pause()
    {
        if (MPlayer.Source is not null)
        {
            _timer.IsEnabled = false;
            MPlayer.Pause();
        }
    }

    [RelayCommand]
    void Rewind()
    {
        MPlayer.Position -= TimeSpan.FromSeconds(10);
    }

    [RelayCommand]
    void FastForward()
    {
        MPlayer.Position += TimeSpan.FromSeconds(10);
    }

[RelayCommand]
    void Next()
    {
        if (MPlayer.Source is not null)
        {
            PlayItem(CurrentMediaList.GetNextItem());
        }
    }

[RelayCommand]
    void Previous()
    {
        if (MPlayer.Source is not null)
        {
            PlayItem(CurrentMediaList.GetPreviousItem());
        }
    }

    [RelayCommand]
    void LoadMedia()
    {
PlaylistManager listManager = new(CurrentMediaList);                
        if (listManager.ShowDialog() == true)
        {
            CurrentMediaList.Items = listManager.Playlist.Items;
            CurrentMediaList.CurrentIndex = listManager.Playlist.CurrentIndex;            
            PlayItem(CurrentMediaList.CurrentItem);        
        }
    }

}
