namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    DispatcherTimer _timer;
    PlaybackList _mediaList;

public     ObservableCollection<MediaItem> MediaItems => _mediaList.Items;

    public PlayerViewModel(PlaybackList playbackList)
    {        
        _mediaList = playbackList;
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

        GetMediaStatus();
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

    void ResetPlayer()
    {
        _mediaList.CurrentIndex = 0;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;        
        GetMediaStatus();
    }    

    void GetMediaStatus() =>    
         DisplayStatus = (MPlayer.Source is null) ? "There is no media loaded." : _mediaList.GetCurrentItem().DisplayName;

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
    void PlayNext() => PlayItem(_mediaList.GetNextItem());

    [RelayCommand]
    void PlayPrevious() => PlayItem(_mediaList.GetPreviousItem());

    [RelayCommand]
    void RemoveItem()
    {       
        if (SelectedItem is MediaItem item)
        {
            _mediaList.RemoveItem(item);            

            if (_mediaList.Items.Count == 0)
                {
                    ResetPlayer();
                    return;
                }

            //PlayItem(_mediaList.GetCurrentItem());
        }
    }

    [RelayCommand]
    void PickFiles()
    {
        var pickedFiles = new FileOpenService().PickMediaFiles();

        if (pickedFiles.Count > 0)
            _mediaList.AddItems(pickedFiles);
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
        if (SelectedItem is MediaItem item)
        new ClipBoardService().Copy(item.FullPath);
        else
            MessageBox.Show("There is no media item selected.");
    }

    [RelayCommand]
    void CopyAllItems()
    {
        if (MediaItems.Count > 0)
        {
            List<string> filePaths = new();

            foreach (var item in MediaItems)
                filePaths.Add(item.FullPath);

            new ClipBoardService().CopyAll(filePaths);            
        }
        else
            MessageBox.Show("There are no media items to copy.");
    }

    [RelayCommand]
    void PasteItems()
    {
        ClipBoardService clipBoard = new();
var files = clipBoard.Paste();

        if (files is not null)
            _mediaList.AddItems(files);        
    }

    [RelayCommand]
    void LoadPlaylist()
    {
        var listVM = new ListManagerViewModel();
        var listManagerView = new ListManagerWindow(listVM);

        if (listManagerView.ShowDialog() == true) 
        {            
                _mediaList.Items.Clear();            
        }
    }

    [RelayCommand]
    void SavePlaylist()
{

}
}
