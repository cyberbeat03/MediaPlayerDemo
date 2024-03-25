using WinMix.Services;

namespace WinMix.ViewModels;

public partial class MainViewModel : MainViewModelBase
{    
    private DispatcherTimer _timer = new();

    public MainViewModel()
    {
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if ((currentItem is not null) && (MPlayer.Source != currentItem.MediaUri))
        {
            MPlayer.Source = currentItem.MediaUri;
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
    }

    [RelayCommand]
    void Play()
    {
        if (MPlayer.Source is not null)
        {
            _timer.Start();
            MPlayer.Play();
        }
    }

    [RelayCommand]
  void Pause()
    {
        if (MPlayer.Source is not null)
        {
            _timer.Stop();            
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
    void PickMediaFiles()
    {
        FileOpenService fileService = new();

        IList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count != 0)
        {
            CurrentMediaList.AddItems(pickedFiles);
            MPlayer.Source = new Uri(pickedFiles[0]);
            Play();
            GetMediaStatus();
        }
    }

    [RelayCommand]
    void PlaySelectedItem()
    {        
        PlayItem(SelectedItem);
    }

    [RelayCommand]
    void RemoveItem()
    {        
        if (SelectedItem is not null)
        {
            if (MPlayer.Source == SelectedItem.MediaUri)
            {
                MPlayer.Stop();
                _timer.Stop();
                MPlayer.Source = null;
                GetMediaStatus();
            }
            CurrentMediaList.Items.Remove(SelectedItem);
        }

    }

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = CurrentMediaList.Items.IndexOf(SelectedItem);
            if (currentPosition > 0)
            CurrentMediaList.Items.Move(currentPosition,     currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = CurrentMediaList.Items.IndexOf(SelectedItem);
            if (currentPosition < CurrentMediaList.Items.Count - 1)
            CurrentMediaList.Items.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
if (SelectedItem is not null)
            {
                ClipBoardService clipboard = new();
                clipboard.Copy(SelectedItem.MediaPath);
            }
            }

[RelayCommand]
void CopyAllItems()
        {
            if (CurrentMediaList.Items.Count > 0)
            {
                List<string> filePaths = new();

                foreach (var item in CurrentMediaList.Items)                
                    filePaths.Add(item.MediaPath);                

ClipBoardService clipboard = new();
                                clipboard.CopyAll(filePaths);
            }       
        }

        [RelayCommand]
        void PasteItems()
        {
            ClipBoardService clipBoard = new();
            IList<string>? returnedFiles = clipBoard.Paste();
            if (returnedFiles is not null)
                CurrentMediaList.AddItems(returnedFiles);
                }

}
