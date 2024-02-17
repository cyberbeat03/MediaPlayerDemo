namespace MediaPlayerDemo.ViewModels;

public partial class MainViewModelBase : ObservableObject
{
    [ObservableProperty] MediaElement _mPlayer = new();
    [ObservableProperty] PlaybackList _currentMediaList = new();
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] string _totalDuration = "00:00";
    [ObservableProperty] string _elapsedTime = "00:00";
    [ObservableProperty] bool _canRepeat;
}
