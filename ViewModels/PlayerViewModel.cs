using WinMix.Models;

namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    [ObservableProperty] MediaElement _mPlayer = new();
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] TimeSpan _totalDuration;
    [ObservableProperty] TimeSpan _elapsedTime;
    DispatcherTimer _timer = new();
    PlaybackList _playback;    

    public PlayerViewModel(PlaybackList playbackList)
    {
        _playback = playbackList;                        
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

    void UpdateStatus() =>
        DisplayStatus = _playback.GetCurrentItem()?.DisplayName ?? "No media is currently loaded.";

    void ResetPlayer()
    {            
            _playback.CurrentIndex = -1;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        MPlayer.SpeedRatio = 1.0;
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
            _playback.RemoveItem(_playback.GetCurrentItem());

            if (_playback.Items.Count == 0)
            {
                ResetPlayer();
                return;
            }
            
                PlayItem(_playback.GetCurrentItem());
    }

    [RelayCommand]
    async Task LoadPlaylist()
    {
        string playlistName = new FileOpenService().PickPlaylistFile();
        if (string.IsNullOrEmpty(playlistName)) return;

        var items = await new ListStorageService().LoadPlaylistAsync(playlistName);
        _playback.Items.Clear();
        foreach (var item in items)
            _playback.AddItem(MediaItem.FromFile(item));
        _playback.Name = playlistName;
        AppTitle = $"{playlistName} -WinMix Desktop";
    }

    [RelayCommand]
    async Task SavePlaylist()
    {
        if (string.IsNullOrEmpty(_playback.Name))
        {
            var inputDialog = new InputTextDialog();
            if (inputDialog.ShowDialog() == true)
                _playback.Name = inputDialog.Response;
            else
                return;
        }

                await new ListStorageService().SavePlaylistAsync(_playback.Name, _playback.GetFiles());
            }

            [RelayCommand]
    void LoadMedia()
    {
        var listVM = new ListManagerViewModel(_playback);
        var listManager = new ListManager(listVM);

        if (listManager.ShowDialog() == true)
        {
            _playback.Items = listVM.MediaItems;
            _playback.CurrentIndex = listVM.MediaItems.IndexOf(listVM.SelectedItem)!;
            }

            PlayItem(_playback.GetCurrentItem());        
    }

}
