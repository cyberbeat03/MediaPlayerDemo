namespace MediaPlayerDemo.ViewModels;

public partial class MainViewModel : MainViewModelBase
{    
    private DispatcherTimer _timer = new();

    public MainViewModel()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {        
                MPlayer.Volume = 0.5;
        MPlayer.Balance = 0;
        MPlayer.SpeedRatio = 1;
        MPlayer.LoadedBehavior = MediaState.Manual;
        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        GetMediaDetails();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)        
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");        
    }

    private void GetMediaDetails()
    {
        if (MPlayer.Source is null)
            DisplayStatus = "There is currently no media loaded";
        else
            DisplayStatus = MPlayer.Source.OriginalString;
    }    

[RelayCommand]
    private void AddMediaFiles()
    {
       FileOpenService fileService = new();

        IList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count != 0)
        {
            CurrentMediaList.AddFiles(pickedFiles);
            PlayItem(CurrentMediaList.CurrentItem);
        }
    }
    
[RelayCommand]
    private void Play()
    {
        if (MPlayer.Source is null) return;

        _timer.IsEnabled = true;
        MPlayer.Play();
    }
    
[RelayCommand]
    private void Pause()
    {
        if (MPlayer.Source is null) return;

        _timer.IsEnabled = false;
        MPlayer.Pause();
    }

[RelayCommand]    
    private void Rewind()
    {
        MPlayer.Position -= TimeSpan.FromSeconds(10);
    }

[RelayCommand]
    private void FastForward()
    {
        MPlayer.Position += TimeSpan.FromSeconds(10);
    }

[RelayCommand]    
    private void Next()
    {
        if (MPlayer.Source is not null)        
            PlayItem(CurrentMediaList.GetNextItem());        
    }

    [RelayCommand]
    private void Previous()
    {
        if (MPlayer.Source is not null)        
            PlayItem(CurrentMediaList.GetPreviousItem());        
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if (currentItem is not null && MPlayer.Source != currentItem.MediaUri)
        {
            MPlayer.Source = currentItem.MediaUri;
            Play();
        }
        GetMediaDetails();
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

}
