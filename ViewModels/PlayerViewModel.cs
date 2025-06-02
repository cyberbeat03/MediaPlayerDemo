namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    [ObservableProperty] MediaElement _mPlayer = new();    
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _totalDuration = "00:00";
    [ObservableProperty] string _elapsedTime = "00:00";
    PlaybackList _mediaList;
    DispatcherTimer _timer;

    public PlayerViewModel(PlaybackList playbackList)
    {        
        _mediaList = playbackList;
        _timer = new();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += OnMediaOpened;        
        MPlayer.MediaEnded += OnMediaEnded;
        MPlayer.MediaFailed += onMediaFailed;

        GetMediaStatus();
    }
    
    void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
    }

    void onMediaFailed(object? sender, ExceptionRoutedEventArgs e)
    {
        MessageBox.Show($"Media failed: {e.ErrorException?.Message}");
        ResetPlayer();        
        e.Handled = true;
    }        

    void OnMediaOpened(object? sender, RoutedEventArgs e)
    {
        TotalDuration = MPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        _timer.Start();
    }

    void OnMediaEnded(object? sender, RoutedEventArgs e)
    {
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = "00:00";
        if (CanRepeat)
            Play();
        else
            PlayNext();            
    }

    void ResetPlayer()
    {
        _mediaList.CurrentIndex = 0;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = "00:00";
        TotalDuration = "00:00";        
        GetMediaStatus();
    }    

    void GetMediaStatus() =>    
         DisplayStatus = (MPlayer.Source is null) ? "There is no media loaded." : MPlayer.Source.OriginalString;    

    void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.UriPath))
        {
            MPlayer.Source = currentItem.UriPath;
            Play();
            GetMediaStatus();
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
    void Rewind() => MPlayer.Position -= TimeSpan.FromSeconds(10);

    [RelayCommand]
    void FastForward() => MPlayer.Position += TimeSpan.FromSeconds(10);

    [RelayCommand]
    void PlayNext() => PlayItem(_mediaList.GetNextItem());

    [RelayCommand]
    void PlayPrevious() => PlayItem(_mediaList.GetPreviousItem());

    [RelayCommand]
    void OpenFiles()
    {
        FileOpenService fileService = new();
        IReadOnlyList<string> pickedFiles = fileService.PickMediaFiles();
        if (pickedFiles.Count > 0)
        {
            _mediaList.AddItems(pickedFiles);
            PlayItem(_mediaList.GetCurrentItem());
        }        
    }

    [RelayCommand]
    void RemoveItem()
    {
        MediaItem? item = _mediaList.GetCurrentItem();

        if (item is not null)
        {
            _mediaList.RemoveItem(item);            

            if (_mediaList.Items.Count == 0)
                {
                    ResetPlayer();
                    return;
                }

            PlayItem(_mediaList.GetCurrentItem());
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        MediaItem? currentItem = _mediaList.GetCurrentItem();

        if (currentItem is MediaItem item)
        {
            ClipBoardService clipboard = new();
            clipboard.Copy(item.FullPath);            
        }
        else            
            MessageBox.Show("There is no media to copy.");        
    }

    [RelayCommand]
    void CopyAllItems()
    {
        if (_mediaList.Items.Count > 0)
        {
            List<string> filePaths = new();

            foreach (var item in _mediaList.Items)
                filePaths.Add(item.FullPath);

            ClipBoardService clipboard = new();
            clipboard.CopyAll(filePaths);            
        }
        else        
            MessageBox.Show("There are no items to copy.");        
    }

    [RelayCommand]
    void PasteItems()
    {
        ClipBoardService clipBoard = new();
        IReadOnlyList<string>? returnedFiles = clipBoard.Paste();
        if (returnedFiles is not null)
        {
            _mediaList.AddItems(returnedFiles);            
        }
    }

    [RelayCommand]
    void LoadMedia()
        {
            var listVM = new ListManagerViewModel(_mediaList);
            var listManagerView = new ListManagerWindow(listVM);

            if (listManagerView.ShowDialog() == true)
            {

                if (_mediaList.Items.Count == 0)
                {
                    ResetPlayer();
                    return;
                }

                if (listVM.SelectedItem is not null)
                {
                    _mediaList.CurrentIndex = listVM.MediaItems.IndexOf(listVM.SelectedItem);
                    PlayItem(_mediaList.GetCurrentItem());
                }
            }
        }
}
