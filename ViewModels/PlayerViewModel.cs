using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;
using WinMix.Models;

namespace WinMix.ViewModels;

public partial class PlayerViewModel : BaseViewModel
{
    DispatcherTimer _timer;
    PlaybackList _playlist;
    bool IsMediaLoaded => MPlayer.Source is not null;

    public ObservableCollection<MediaItem> MediaItems => _playlist.Items;

    public PlayerViewModel(PlaybackList playbackList)
    {
        _playlist = playbackList;
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

    void GetMediaStatus() =>
         DisplayStatus = IsMediaLoaded ? _playlist.GetCurrentItem()?.DisplayName : "There is no media loaded.";

    void ResetPlayer()
    {
        _playlist.CurrentIndex = -1;
        _timer.Stop();
        MPlayer.Stop();
        MPlayer.Source = null;
        ElapsedTime = TimeSpan.Zero;
        TotalDuration = TimeSpan.Zero;
        GetMediaStatus();
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
    void PlaySelected()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);
            _playlist.CurrentIndex = currentPosition;
            PlayItem(item);
        }
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
    void PlayNext() => PlayItem(_playlist.GetNextItem());

    [RelayCommand]
    void PlayPrevious() => PlayItem(_playlist.GetPreviousItem());

    [RelayCommand]
    void RemoveItem()
    {
        if (SelectedItem is MediaItem item)
        {
            _playlist.RemoveItem(item);
            
            
            if (IsMediaLoaded && _playlist.CurrentIndex == -1)
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
            _playlist.AddItems(pickedFiles);
        PlayItem(_playlist.GetCurrentItem());
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
        try
        {
            if (SelectedItem is MediaItem item)
            {
                new ClipBoardService().Copy(item.FullPath);
                MessageBox.Show($"File {item.DisplayName} was copied to the clipboard.");
            }
            else
                MessageBox.Show("No item to copy.");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }                            
    }

    [RelayCommand]
    void CopyAllItems()
    {        
            if (MediaItems.Count == 0)
            {
                MessageBox.Show("There are no items to copy.");
                return;
        }   

        List<string> filePaths = new();

            foreach (var item in MediaItems)
                filePaths.Add(item.FullPath);

            try
            {
                new ClipBoardService().CopyAll(filePaths);
            MessageBox.Show("All files were copied to the clipboard.");
        }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }        
    }

    [RelayCommand]
     void PasteItems()
    {
        try
        {
var files = new ClipBoardService().Paste();           
if (files.Count > 0)                
            _playlist.AddItems(files);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
                }
    }

    [RelayCommand]
    async Task LoadPlaylist()
    {
        try
        {
            var playlists = await new PlaylistService().LoadM3UAsync("MyWinMixPlaylist.m3u8");

            _playlist.Items.Clear();
            _playlist.CurrentIndex = 0;
            AppTitle = "MyWinMixPlaylist -- WinMix Desktop";
            if (playlists.Count > 0)
                _playlist.AddItems(playlists);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }    

    [RelayCommand]
    async Task SavePlaylist()
    {
        if (MediaItems.Count > 0)
        {
            List<string> pathList = new();

            foreach (var item in MediaItems)
                pathList.Add(item.FullPath);

            try
            {
                await new PlaylistService().SaveToM3UAsync("MyWinMixPlaylist.m3u8", pathList);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
            }

}
