namespace WinMix.ViewModels;

public partial class PlayerViewModel : ObservableObject
{
    [ObservableProperty] MediaElement _mPlayer = new();
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = "No media loaded. Press the 'Add' button to get started.";
    [ObservableProperty] string _appTitle = "WinMix Desktop Player";
    [ObservableProperty] TimeSpan _totalDuration = TimeSpan.Zero;
    [ObservableProperty] TimeSpan _elapsedTime = TimeSpan.Zero;
    [ObservableProperty] MediaItem? _selectedItem = null;
    DispatcherTimer _timer = new();
    IPlaybackService _playback;

    public ObservableCollection<MediaItem> MediaItems => _playback.Items;

    public PlayerViewModel(IPlaybackService playbackService)
    {
        _playback = playbackService;
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += OnMediaOpened;
        MPlayer.MediaEnded += OnMediaEnded;
        MPlayer.MediaFailed += (s, e) => DisplayStatus = $"Media failed: {e.ErrorException?.Message}";
    }

    void Timer_Tick(object? s, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position;
    }

    void OnMediaOpened(object? sender, RoutedEventArgs e)
    {
        DisplayStatus = $"Loaded: {_playback.GetCurrentItem()?.DisplayName}" ?? "Media could not be opened.";
        TotalDuration = MPlayer.NaturalDuration.TimeSpan;
        _timer.Start();
    }

    void OnMediaEnded(object? sender, RoutedEventArgs e)
    {
        DisplayStatus = $"End of {_playback.GetCurrentItem()?.DisplayName}" ?? "Media has ended.";
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = TimeSpan.Zero;
        if (CanRepeat)
            MPlayer.Play();
        else
            PlayNext();
    }

    void ResetPlayer()
    {
        _playback.CurrentIndex = -1;
        _playback.Items.Clear();
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        MPlayer.SpeedRatio = 1.0;
        DisplayStatus = "No media currently loaded.";
    }

    void PlayItem(MediaItem? currentItem)
    {
        if (currentItem is not null)
        {
            MPlayer.Source = currentItem.UriPath;
            MPlayer.Play();
        }
    }

    [RelayCommand]
    void Play() => MPlayer.Play();

    [RelayCommand]
    void Pause() => MPlayer.Pause();

    [RelayCommand]
    void Stop() => MPlayer.Stop();

    [RelayCommand]
    void Rewind() => MPlayer.Position -= TimeSpan.FromSeconds(10);

    [RelayCommand]
    void SpeedUp()
    {
        double fastest = 1.3;

        if (MPlayer.SpeedRatio <= fastest) MPlayer.SpeedRatio += 0.1;
    }

    [RelayCommand]
    void SlowDown()
    {
        double slowest = 0.7;

        if (MPlayer.SpeedRatio >= slowest) MPlayer.SpeedRatio -= 0.1;
    }

    [RelayCommand]
    void FastForward() => MPlayer.Position += TimeSpan.FromSeconds(10);

    [RelayCommand]
    void PlayNext() => PlayItem(_playback.GetNextItem());

    [RelayCommand]
    void PlayPrevious() => PlayItem(_playback.GetPreviousItem());

    [RelayCommand]
    void PlaySelected()
    {
        if (SelectedItem is MediaItem item)
        {
            _playback.CurrentIndex = _playback.Items.IndexOf(item);
            PlayItem(item);
        }
    }

    [RelayCommand]
    void OpenFiles()
    {
        var pickedFiles = new FileOpenService().PickMediaFiles();
        if (pickedFiles.Count() > 0)
            foreach (var file in pickedFiles)
                _playback.AddItem(MediaItem.FromFile(file));
        if (MPlayer.Source is null)
            PlayItem(_playback.GetCurrentItem());
    }

    [RelayCommand]
    void RemoveItem()
    {
        if (SelectedItem is MediaItem item)
        {
            _playback.RemoveItem(item);

            if (_playback.Items.Count == 0)
                ResetPlayer();
            else
                PlayItem(_playback.GetCurrentItem());
        }
    }
    
[RelayCommand]
    void ShowAbout()
    {
        var about = new AboutWindow();
        about.ShowDialog();
    }

}
