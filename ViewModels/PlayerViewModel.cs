namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    DispatcherTimer _timer;
    PlaybackList _playlist;    

    public ObservableCollection<MediaItem> MediaItems => _playlist.Items;

    public PlayerViewModel(PlaybackList playbackList)
    {
        _playlist = playbackList;
        AppTitle = $"{_playlist.Name} - WinMix Desktop";
        _timer = new();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += OnMediaOpened;
        MPlayer.MediaEnded += OnMediaEnded;
        MPlayer.MediaFailed += (s, e) =>
        {
            MessageBox.Show($"Media failed: {e.ErrorException?.Message}");
            ResetPlayer();
        };   

        UpdateStatus();
    }

    void Timer_Tick(object? s, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position;
    }

    void OnMediaOpened(object? sender, RoutedEventArgs e)
    {
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

    void UpdateStatus()
    {
        if (MPlayer.Source is null)
        {
            DisplayStatus = "No media is currently loaded.";
            return;
        }

        DisplayStatus = (_playlist.GetCurrentItem() is null) ? "There is no current item" : _playlist.GetCurrentItem().DisplayName;        
    }

    void ResetPlayer()
    {
        _playlist.CurrentIndex = -1;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        UpdateStatus();
    }        
        
        void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.UriPath))
        {
            MPlayer.Source = currentItem.UriPath;
            Play();
            UpdateStatus();
        }
    }

    [RelayCommand]
    void Play()
    {
        _timer.IsEnabled = true;
        MPlayer.Play();
    }

    [RelayCommand]
    void PlaySelected()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);
            _playlist.CurrentIndex = currentPosition;
            PlayItem(item);
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
    void FastForward() => MPlayer.Position += TimeSpan.FromSeconds(10);

    [RelayCommand]
    void PlayNext() => PlayItem(_playlist.GetNextItem());

    [RelayCommand]
    void PlayPrevious() => PlayItem(_playlist.GetPreviousItem());

    [RelayCommand]
    void RemoveItem()
    {
        if (SelectedItem is MediaItem item)
        {
            _playlist.RemoveItem(item);            
            
            if (_playlist.Items.Count == 0)            
                ResetPlayer();                            
        }
    }

    [RelayCommand]
    void PickFiles()
    {                        
            try
            {
            var pickedFiles = new FileOpenService().PickMediaFiles();
            if (pickedFiles.Count > 0)
            {
                _playlist.AddFiles(pickedFiles);
                PlayItem(_playlist.GetCurrentItem());
            }
        }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
    }

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);
            if (currentPosition > 0)
                MediaItems.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);

            if (currentPosition < MediaItems.Count - 1)
                MediaItems.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        try
        {
            if (SelectedItem is MediaItem item)
            {
                new ClipBoardService().Copy(item.FullPath);
                MessageBox.Show($"File {item.DisplayName} was copied to the clipboard.");
            }
            else
                MessageBox.Show("No item to copy.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }                            
    }

    [RelayCommand]
    void CopyAllItems()
    {        
            if (MediaItems.Count == 0)
            {
                MessageBox.Show("There are no items to copy.");
                return;
        }   

            try
            {
                new ClipBoardService().CopyAll(_playlist.GetFiles());
            MessageBox.Show("All files were copied to the clipboard.");
        }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }        
    }

    [RelayCommand]
     void PasteItems()
    {
        try
        {
var files = new ClipBoardService().Paste();           
if (files.Count > 0)                
            _playlist.AddFiles(files);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
                }
    }

    [RelayCommand]
    async Task LoadPlaylist()
    {
        try
        {
            string playlistFileName = new FileOpenService().PickPlaylistFile();
            if (string.IsNullOrEmpty(playlistFileName)) return;
        
            var mediaFiles = await new PlaylistService().LoadAsync(playlistFileName);
            _playlist.Items.Clear();
            _playlist.CurrentIndex = -1;
            if (mediaFiles.Count > 0)
            {                                                
                _playlist.AddFiles(mediaFiles);                
                _playlist.Name = Path.GetFileNameWithoutExtension(playlistFileName);
                AppTitle = $"{_playlist.Name} -WinMix Desktop";
                PlayItem(_playlist.GetCurrentItem());
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }    

    [RelayCommand]
    async Task SavePlaylist()
    {        
            try
            {                
                    await new PlaylistService().SaveAsync(_playlist.Name, _playlist.GetFiles());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }            

}
