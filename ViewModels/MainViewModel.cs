namespace MediaPlayerDemo.ViewModels;

public partial class MainViewModel : MainViewModelBase
{
    [ObservableProperty] private MediaElement _mPlayer = new();
    [ObservableProperty] private PlaybackList _currentMediaList = new();
    [ObservableProperty] private string _displayStatus = string.Empty;
[ObservableProperty] private string _totalDuration = "00:00";
[ObservableProperty] private string _elapsedTime = "00:00";
private DispatcherTimer _timer = new();

    public bool CanRepeat { get; set; }


    public MainViewModel()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {        
                MPlayer.Volume = 0.5;
        MPlayer.Balance = 0;
        MPlayer.SpeedRatio = 1;
        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        GetMediaDetails();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
        {
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
        }
    }

    public void ChangeSpeed(double newValue)
    {
        MPlayer.SpeedRatio = newValue;
    }

    public void AddMediaFiles()
    {
        FileOperations fileOperations = new();

        IList<string> pickedFiles = fileOperations.PickMediaFiles();

        if (pickedFiles.Count != 0)
        {
            CurrentMediaList.AddFilesToList(pickedFiles);
            PlayItem(CurrentMediaList.CurrentItem);
        }
    }

    public void GetMediaDetails()
    {
        if (MPlayer.Source is null)
        {
            DisplayStatus = "Nothing to play";
        }
        else
        {
            DisplayStatus = $"{MPlayer.Source}";
        }
    }

    public void Play()
    {
        if (MPlayer.Source is null) return;

        if (_timer.IsEnabled == false) _timer.Start();

        MPlayer.Play();
    }

    public void Pause()
    {
        if (MPlayer.Source is null) return;

        _timer.Stop();
        MPlayer.Pause();
    }

    public void Rewind()
    {
        MPlayer.Position -= TimeSpan.FromSeconds(10);
    }

    public void FastForward()
    {
        MPlayer.Position += TimeSpan.FromSeconds(10);
    }

    public void PlayNext()
    {
        if (MPlayer.Source is not null)
        {
            PlayItem(CurrentMediaList.GetNextItem());
        }
    }

    public void PlayPrevious()
    {
        if (MPlayer.Source is not null)
        {
            PlayItem(CurrentMediaList.GetPreviousItem());
        }
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if (currentItem != null && MPlayer.Source != currentItem.MediaUri)
        {
            MPlayer.Source = currentItem.MediaUri;
            Play();
        }
        GetMediaDetails();
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
        {
            Play();
        }
        else
        {
            PlayNext();
        }
    }
}
