using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    [ObservableProperty] MediaElement _mPlayer = new();    
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _totalDuration = "00:00";
    [ObservableProperty] string _elapsedTime = "00:00";
    PlaybackList _mediaList = new();
    DispatcherTimer _timer = new();

    public PlayerViewModel()
    {        
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;

        MPlayer.LoadedBehavior = MediaState.Manual;        
        MPlayer.Volume = 0.5;
        MPlayer.Balance = 0;
        MPlayer.SpeedRatio = 1;        

        MPlayer.MediaOpened += Media_Opened;
        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;
        MPlayer.MediaFailed += Media_Failed;
        GetMediaStatus();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
    }

    private void GetMediaStatus()
    {        
        if (MPlayer.Source is null)
            DisplayStatus = "There is currently no media loaded.";
                else
            DisplayStatus = MPlayer.Source.OriginalString;
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.UriPath))
        {
            MPlayer.Source = currentItem.UriPath;
            Play();            
            GetMediaStatus();
        }
    }

    private void Media_Opened(object sender, RoutedEventArgs e)
    {
        TotalDuration = MPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        _timer.Start();
    }

    private void Media_Ended(object sender, RoutedEventArgs e)
    {
        _timer.Stop();
        MPlayer.Stop();
        ElapsedTime = "00:00";
        if (CanRepeat)
            Play();
        else
            Next();
    }

    private void Media_Failed(object sender, RoutedEventArgs e)
    {
DisplayStatus = "Could not load media. Make sure the file path is valid and in a supported format.";
        MPlayer.Source = null;
        _timer.Stop();        
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
    void LoadFiles()
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
            ListManagerViewModel listVM = new(_mediaList);
        ListManagerDialog listManager = new(listVM);        

        if (listManager.ShowDialog() == true)
        {
_mediaList.Items = listVM.MediaItems;            

if (listVM.SelectedItem is not null)
            _mediaList.CurrentIndex = listVM.MediaItems.IndexOf(listVM.SelectedItem);
else
                _mediaList.CurrentIndex = 0;

            PlayItem(_mediaList.CurrentItem);        
        }
    }

}
