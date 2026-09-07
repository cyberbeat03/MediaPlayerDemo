namespace WinMix.ViewModels;

public partial class PlayerViewModel : ObservableObject, IDisposable
{
    [ObservableProperty] string _displayStatus = "No media loaded. Press the 'Add' button to get started.";
    [ObservableProperty] bool _isPlaying = false;
    [ObservableProperty] TimeSpan _totalDuration = TimeSpan.Zero;
    [ObservableProperty] TimeSpan _elapsedTime = TimeSpan.Zero;
    [ObservableProperty] MediaItem? _selectedItem = null;
    [ObservableProperty] string _titleBar = "WinMix Desktop Music Player";
    bool _disposed;

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
        
        _playbackService.PositionChanged += PlaybackService_PositionChanged;
        _playbackService.MediaOpened += PlaybackService_MediaOpened;
        _playbackService.MediaEnded += PlaybackService_MediaEnded;
        _playbackService.MediaFailed += PlaybackService_MediaFailed;
        _playbackService.CurrentItemChanged += PlaybackService_CurrentItemChanged;
        _playbackService.PlayingChanged += PlaybackService_PlayingChanged;
    }

    void PlaybackService_PositionChanged(object? s, TimeSpan position)
    {
        ElapsedTime = position;
    }

    void PlaybackService_MediaOpened(object? s, TimeSpan duration)
    {
        DisplayStatus = $"Loaded: {_playbackService.GetCurrentItem()?.DisplayName}" ?? "Media could not be opened.";
        TotalDuration = duration;
    }

    void PlaybackService_MediaFailed(object? s, Exception e)
    {
        DisplayStatus = $"Media failed: {e.Message}";
    }

    void PlaybackService_MediaEnded(object? s, EventArgs e)
    {
        DisplayStatus = $"End of {_playbackService.GetCurrentItem()?.DisplayName}" ?? "Media has ended.";
        ElapsedTime = TimeSpan.Zero;
        _playbackService.PlayNext();
    }

    void PlaybackService_CurrentItemChanged(object? s, EventArgs e)
    {
        SelectedItem = _playbackService.GetCurrentItem();
    }

    void PlaybackService_PlayingChanged(object? s, bool playing)
    {
        IsPlaying = playing;
        var current = _playbackService.GetCurrentItem();
        if (current != null)        
            DisplayStatus = playing ? $"Playing {current.DisplayName}" : $"{current.DisplayName} (not playing)";        
    }

    void ResetPlayer()
    {
        _playbackService.CurrentIndex = -1;
        _playbackService.Items.Clear();
        _playbackService.Stop();
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        _playbackService.SpeedRatio = 1.0;
        DisplayStatus = "No media currently loaded.";
    }

    /*
    void PlayItem(MediaItem? currentItem)
    {
        if (currentItem is not null)
        {
            var idx = _playbackService.Items.IndexOf(currentItem);
            if (idx >= 0) _playbackService.CurrentIndex = idx;
            _playbackService.Play();
        }
    }
    */

    [RelayCommand]
    void Play() => _playbackService.Play();

    [RelayCommand]
    void Pause() => _playbackService.Pause();

    [RelayCommand]
    void Stop() => _playbackService.Stop();

    [RelayCommand]
    void Rewind()=>    
        _playbackService.Seek(_playbackService.Position - TimeSpan.FromSeconds(10));    

    [RelayCommand]
    void SpeedUp()
    {
        double fastest = 1.3;

        if (_playbackService.SpeedRatio <= fastest)
            _playbackService.SpeedRatio += 0.1;
    }

    [RelayCommand]
    void SlowDown()
    {
        double slowest = 0.7;

        if (_playbackService.SpeedRatio >= slowest)
            _playbackService.SpeedRatio -= 0.1;
    }

    [RelayCommand]
    void FastForward() =>
        _playbackService.Seek(_playbackService.Position + TimeSpan.FromSeconds(10));

    [RelayCommand]
    void PlayNext() => _playbackService.PlayNext();

    [RelayCommand]
    void PlayPrevious() => _playbackService.PlayPrevious();

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
                _playbackService.Play();
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        if (SelectedItem is MediaItem item)
            _clipBoardService.Copy(item.FullPath);
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = _clipBoardService.Paste();
        foreach (var item in pastedItems)
            _playbackService.AddItem(MediaItem.FromFile(item));
    }

    [RelayCommand]
    void PlaySelected()
    {
        if (SelectedItem is MediaItem item)
        {
            _playbackService.CurrentIndex = _playbackService.Items.IndexOf(item);
            _playbackService.Play();
        }
    }

    [RelayCommand]
    void OpenFiles()
    {
        var pickedFiles = _fileOpenService.PickMediaFiles();
        if (pickedFiles.Count() > 0)
            foreach (var file in pickedFiles)
                _playbackService.AddItem(MediaItem.FromFile(file));
        if (_playbackService.Items.Count > 0 && !_playbackService.IsPlaying)
            _playbackService.Play();
    }

    [RelayCommand]
    void ShowAbout()
    {
        _windowDisplayService.ShowAboutDialog();
    }

    [RelayCommand]
    async Task LoadList()
    {
        string? playlistFile = await _windowDisplayService.PickPlaylistFileAsync();
        if (string.IsNullOrEmpty(playlistFile)) return;

        _playbackService.Items.Clear();
        _playbackService.Name = Path.GetFileNameWithoutExtension(playlistFile);
        TitleBar = $"{_playbackService.Name} - WinMix Desktop";
        var items = await _storageService.LoadPlaylistAsync(playlistFile);
        foreach (var item in items)
            _playbackService.AddItem(item);

        if (_playbackService.Items.Count > 0)
        {
            _playbackService.CurrentIndex = 0;
            _playbackService.Play();
        }
        else
        {
            ResetPlayer();
        }
    }

    [RelayCommand]
    async Task SaveList()
    {
        if (_playbackService.Items.Count == 0) return;

        if (_playbackService.Name == string.Empty)
        {
            string input = _windowDisplayService.ShowInputDialog();
            if (string.IsNullOrEmpty(input)) return;

            _playbackService.Name = input;
            TitleBar = $"{_playbackService.Name} - WinMix Desktop";
        }
        await _storageService.SavePlaylistAsync($"{_playbackService.Name}.wmx", _playbackService.Items);
    }

    [RelayCommand]
    async Task CreateNewList()
    {
        string input = _windowDisplayService.ShowInputDialog(); if (string.IsNullOrWhiteSpace(input)) return;

        _playbackService.Name = input;
        TitleBar = $"{_playbackService.Name} - List Manager";
        ResetPlayer();
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
                _playbackService.PositionChanged -= PlaybackService_PositionChanged;
                _playbackService.MediaOpened -= PlaybackService_MediaOpened;
                _playbackService.MediaEnded -= PlaybackService_MediaEnded;
                _playbackService.MediaFailed -= PlaybackService_MediaFailed;
                _playbackService.CurrentItemChanged -= PlaybackService_CurrentItemChanged;
                _playbackService.PlayingChanged -= PlaybackService_PlayingChanged;
            }
            catch { }
        }

        _disposed = true;
    }

}
