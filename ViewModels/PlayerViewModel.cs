namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    [ObservableProperty] MediaElement _mPlayer = new();    
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _totalDuration = "00:00";
    [ObservableProperty] string _elapsedTime = "00:00";
    PlaybackList _mediaList = new();
readonly DispatcherTimer _timer = new();

    public PlayerViewModel()
    {                                
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
        MPlayer.Source = null;
        ElapsedTime = "00:00";
        TotalDuration = "00:00";
        _timer.Stop();
        GetMediaStatus();
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
            Next();
    }

    void GetMediaStatus()
    {        
        if (MPlayer.Source is null)
            DisplayStatus = "There is currently no media loaded.";
                else
            DisplayStatus = MPlayer.Source.OriginalString;
    }

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
            PlayItem(_mediaList.NextItem);        
    }

[RelayCommand]
    void Previous()
    {        
            PlayItem(_mediaList.PreviousItem);        
    }

    [RelayCommand]
    void OpenFiles()
    {
        FileOpenService fileService = new();
        IReadOnlyList<string> pickedFiles = fileService.PickMediaFiles();
        if (pickedFiles.Count > 0)
        {
            foreach (string mediaFile in pickedFiles)
            {
                _mediaList.Items.Add(new MediaItem(new FileInfo(mediaFile)));
        }

            _mediaList.CurrentIndex = 0;
        PlayItem(_mediaList.CurrentItem);
        }        
    }

[RelayCommand]
    void LoadMedia()
    {
             var listVM = new ListManagerViewModel(_mediaList);
        var listManagerView = new ListManagerDialog(listVM);

        if (listManagerView.ShowDialog() == true)
        {
            _mediaList.Items = listVM.MediaItems;

            if (listVM.SelectedItem is not null)
            {
                _mediaList.CurrentIndex = listVM.MediaItems.IndexOf(listVM.SelectedItem);
                PlayItem(_mediaList.CurrentItem);
            }
            else if (_mediaList.Items.Count > 0)
                    _mediaList.CurrentIndex = 0;
                else
                    _mediaList.CurrentIndex = -1;
            }
        }                        

}
