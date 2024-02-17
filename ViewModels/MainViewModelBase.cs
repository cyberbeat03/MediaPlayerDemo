namespace MediaPlayerDemo.ViewModels;

public partial class MainViewModelBase : ObservableObject
{
    [ObservableProperty] MediaElement _mPlayer = new();
    [ObservableProperty] PlaybackList _currentMediaList = new();
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _totalDuration = "00:00";
    [ObservableProperty] string _elapsedTime = "00:00";
    [ObservableProperty] bool _canRepeat;

    public MainViewModelBase()
    {
        InitializePlayer();    
    }

private     void InitializePlayer()
    {
        MPlayer.Volume = 0.5;
        MPlayer.Balance = 0;
        MPlayer.SpeedRatio = 1;
        MPlayer.LoadedBehavior = MediaState.Manual;
        GetMediaDetails();
    }
    
  public void GetMediaDetails()
    {
        if (MPlayer.Source is null)
            DisplayStatus = "There is currently no media loaded";
        else
            DisplayStatus = MPlayer.Source.OriginalString;
    }    

}
