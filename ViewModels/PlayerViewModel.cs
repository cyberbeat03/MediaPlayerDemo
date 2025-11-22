namespace WinMix.ViewModels;

public partial class PlayerViewModel : ObservableObject, IDisposable
{
    [ObservableProperty] private string _displayStatus = "No media loaded. Press the 'Add' button to get started.";
    [ObservableProperty] private TimeSpan _totalDuration = TimeSpan.Zero;
    [ObservableProperty] private TimeSpan _elapsedTime = TimeSpan.Zero;
    [ObservableProperty] private MediaItem? _selectedItem = null;
    [ObservableProperty] private System.Windows.Controls.MediaElement _mPlayer = new();
    bool _disposed;
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
        MPlayer.MediaFailed += OnMediaFailed;
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

    void OnMediaFailed(object? sender, ExceptionRoutedEventArgs e)
    {
        DisplayStatus = $"Media failed: {e.ErrorException?.Message}";
    }

    void OnMediaEnded(object? sender, RoutedEventArgs e)
    {
        DisplayStatus = $"End of {_playback.GetCurrentItem()?.DisplayName}" ?? "Media has ended.";
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = TimeSpan.Zero;
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
    void PlayNext()
    {
        var nextItem = _playback.GetNextItem();
        PlayItem(nextItem);
    }

    [RelayCommand]
    void PlayPrevious()
    {
        var previousItem = _playback.GetPreviousItem();
        PlayItem(previousItem);
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            try
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
            }
            catch { }

            try
            {
                MPlayer.MediaOpened -= OnMediaOpened;
                MPlayer.MediaEnded -= OnMediaEnded;
                MPlayer.MediaFailed -= OnMediaFailed;
                MPlayer.Stop();
                MPlayer.Source = null;
            }
            catch { }
        }

        _disposed = true;
    }

}
