namespace WinMix.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty] MediaElement _mPlayer = new();
    [ObservableProperty] bool _canRepeat = false;
    [ObservableProperty] string _displayStatus = string.Empty;
    [ObservableProperty] TimeSpan _totalDuration;
    [ObservableProperty] TimeSpan _elapsedTime;
    [ObservableProperty] string _appTitle = "WinMix Desktop";
    [ObservableProperty] MediaItem? _selectedItem;
}
