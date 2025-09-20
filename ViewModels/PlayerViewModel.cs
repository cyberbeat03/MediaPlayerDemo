namespace WinMix.ViewModels;

public partial class PlayerViewModel : ObservableObject
{    
    [ObservableProperty] MediaElement _mPlayer = new();    
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _appTitle = "WinMix Desktop";
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
        MPlayer.MediaFailed += (s, e) =>
        {
DisplayStatus =             $"Media failed: {e.ErrorException?.Message}";
            ResetPlayer();
        };

        if (_playback.CurrentIndex != 0)
        PlayItem(_playback.GetCurrentItem());
        AppTitle = $"Playlist: {_playback.Name} - WinMix Desktop";
    }

    void Timer_Tick(object? s, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position;
    }

    void OnMediaOpened(object? sender, RoutedEventArgs e)
    {
        DisplayStatus = _playback.GetCurrentItem().DisplayName;
        TotalDuration = MPlayer.NaturalDuration.TimeSpan;
        _timer.Start();
    }

    void OnMediaEnded(object? sender, RoutedEventArgs e)
    {
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = TimeSpan.Zero;
        if (CanRepeat)
            Play();
        else
            PlayNext();
    }
    
        
    void ResetPlayer()
    {            
            _playback.CurrentIndex = -1;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        MPlayer.SpeedRatio = 1.0;
        DisplayStatus = "No media is currently loaded.";
    }

    void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.UriPath))
        {
            MPlayer.Source = currentItem.UriPath;
            Play();            
        }
    }

    [RelayCommand]
    void Play()
    {
        _timer.IsEnabled = true;
        MPlayer.Play();
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
    void Stop()
    {
        if (MPlayer.Source is not null)
        {
            _timer.IsEnabled = false;
            MPlayer.Stop();
            ElapsedTime = TimeSpan.Zero;
        }
    }

    [RelayCommand]
    void Rewind() => MPlayer.Position -= TimeSpan.FromSeconds(10);

    [RelayCommand]
    void SetSpeed(string speedRatio) =>
    MPlayer.SpeedRatio = double.Parse(speedRatio);

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
            PlayItem(_playback.GetCurrentItem());        
    }

        [RelayCommand]
    void RemoveItem()
    {        
if (SelectedItem is MediaItem item)
            _playback.RemoveItem(item);                                               
    }

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = _playback.Items.IndexOf(item);
            if (currentPosition > 0)
                _playback.Items.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = _playback.Items.IndexOf(item);

            if (currentPosition < _playback.Items.Count - 1)
                _playback.Items.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        if (SelectedItem is MediaItem item)
            new ClipBoardService().Copy(item.FullPath);
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = new ClipBoardService().Paste();
        foreach (var item in pastedItems)
            _playback.AddItem(MediaItem.FromFile(item));
    }

    [RelayCommand]
    async Task SaveList()
    {
        await new ListStorageService().SavePlaylistAsync(_playback.Name, _playback.GetFilePaths());
    }

    [RelayCommand]
    async Task LoadList()
    {
        string playlistFile = new FileOpenService().PickPlaylistFile();
        if (!string.IsNullOrEmpty(playlistFile))
        {
            _playback.Items.Clear();
            _playback.Name = Path.GetFileNameWithoutExtension(playlistFile);
            AppTitle = $"Playlist: {_playback.Name} - WinMix Desktop";
            var items = await new ListStorageService().LoadPlaylistAsync(playlistFile);            
            foreach (var item in items)
                _playback.AddItem(MediaItem.FromFile(item));
        }
    }

    [RelayCommand]
    async Task NewList()
    {
        var inputDialog = new InputTextDialog();
        if (inputDialog.ShowDialog() == true)
        {
            string input = inputDialog.Response;

            _playback.Items.Clear();
            _playback.Name = "input";
            AppTitle = $"Playlist: {_playback.Name} - WinMix Desktop";
            await new ListStorageService().SavePlaylistAsync(_playback.Name, _playback.GetFilePaths());
        }
    }

    [RelayCommand]
    void ShowAbout()
    {
        var about = new AboutWindow();
        about.ShowDialog();
    }

    }
