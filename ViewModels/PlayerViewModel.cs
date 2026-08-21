namespace WinMix.ViewModels;

public partial class PlayerViewModel : ObservableObject, IDisposable
{
    [ObservableProperty] string _displayStatus = "No media loaded. Press the 'Add' button to get started.";
    [ObservableProperty] TimeSpan _totalDuration = TimeSpan.Zero;
    [ObservableProperty] TimeSpan _elapsedTime = TimeSpan.Zero;
    [ObservableProperty] MediaItem? _selectedItem = null;
    [ObservableProperty] System.Windows.Controls.MediaElement _mPlayer = new();
    [ObservableProperty] string _titleBar = "WinMix Desktop Music Player";
    bool _disposed;
    DispatcherTimer _timer = new();

    readonly IPlaybackService _playbackService;
    readonly IFileOpenService _fileOpenService;
    readonly IClipBoardService _clipBoardService;
    readonly IStorageService _storageService;    
    readonly IWindowDisplayService _windowDisplayService;

    public ObservableCollection<MediaItem> MediaItems => _playbackService.Items;

    public PlayerViewModel(IPlaybackService playbackService, IFileOpenService fileOpenService, IClipBoardService clipBoardService, IStorageService storageService, IWindowDisplayService windowDisplayService)
    {
        _playbackService = playbackService ?? throw new ArgumentNullException(nameof(playbackService));
        _fileOpenService = fileOpenService ?? throw new ArgumentNullException(nameof(fileOpenService));
        _clipBoardService = clipBoardService ?? throw new ArgumentNullException(nameof(clipBoardService));
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        _windowDisplayService = windowDisplayService ?? throw new ArgumentNullException(nameof(windowDisplayService));

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
        DisplayStatus = $"Loaded: {_playbackService.GetCurrentItem()?.DisplayName}" ?? "Media could not be opened.";
        TotalDuration = MPlayer.NaturalDuration.TimeSpan;
        _timer.Start();
    }

    void OnMediaFailed(object? sender, ExceptionRoutedEventArgs e)
    {
        DisplayStatus = $"Media failed: {e.ErrorException?.Message}";
    }

    void OnMediaEnded(object? sender, RoutedEventArgs e)
    {
        DisplayStatus = $"End of {_playbackService.GetCurrentItem()?.DisplayName}" ?? "Media has ended.";
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = TimeSpan.Zero;
        PlayNext();
    }

    void ResetPlayer()
    {
        _playbackService.CurrentIndex = -1;
        _playbackService.Items.Clear();
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
        var nextItem = _playbackService.GetNextItem();
        PlayItem(nextItem);
    }

    [RelayCommand]
    void PlayPrevious()
    {
        var previousItem = _playbackService.GetPreviousItem();
        PlayItem(previousItem);
    }

    [RelayCommand]
    void MoveItemUp() => _playbackService.MoveUp(SelectedItem);
    [RelayCommand]
    void MoveItemDown() => _playbackService.MoveDown(SelectedItem);

    [RelayCommand]
    void RemoveItem()
    {
        if (SelectedItem is MediaItem item)
        {
            _playbackService.RemoveItem(item);

            if (_playbackService.Items.Count == 0)
                ResetPlayer();
            else
                PlayItem(_playbackService.GetCurrentItem());
        }
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = _clipBoardService.Paste();
        foreach (var item in pastedItems)
            _playbackService.Items.Add(MediaItem.FromFile(item));
    }

    [RelayCommand]
    void PlaySelected()
    {
        if (SelectedItem is MediaItem item)
        {
            _playbackService.CurrentIndex = _playbackService.Items.IndexOf(item);
            PlayItem(item);
        }
    }

    [RelayCommand]
    void OpenFiles()
    {
        var pickedFiles = _fileOpenService.PickMediaFiles();
        if (pickedFiles.Count() > 0)
            foreach (var file in pickedFiles)
                _playbackService.AddItem(MediaItem.FromFile(file));
        if (MPlayer.Source is null)
            PlayItem(_playbackService.GetCurrentItem());
    }

    [RelayCommand]
    void OpenListManager() => _windowDisplayService.ShowListManager();

    [RelayCommand]
    void ShowAbout()
    {
        var about = new AboutWindow();
        about.ShowDialog();
    }

    [RelayCommand]
    async void LoadListAsync()
    {
        string playlistFile = _fileOpenService.PickPlaylistFile();
        if (!string.IsNullOrEmpty(playlistFile))
        {
            _playbackService.Items.Clear();

            _playbackService.Name = Path.GetFileNameWithoutExtension(playlistFile);
            TitleBar = $"{_playbackService.Name} - WinMix Desktop";
            var items = await _storageService.LoadPlaylistAsync(playlistFile);
            foreach (var item in items)
                _playbackService.AddItem(item);
        }
    }

    [RelayCommand]
    async void SaveListAsync()
    {        
        if (_playbackService.Items.Count == 0)
        {
            MessageBox.Show("No media items to save.", "Save Playlist");
            return;
        }

        var inputDialog = new InputTextDialog();
        
        if (inputDialog.ShowDialog() == true)
        {
            string input = inputDialog.Response;
            
            _playbackService.Name = input;
            TitleBar = $"{_playbackService.Name} - WinMix Desktop";
            await _storageService.SavePlaylistAsync($"{input}.wmx", _playbackService.Items);
        }
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