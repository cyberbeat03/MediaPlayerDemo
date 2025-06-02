namespace WinMix.Models;

public interface IPlaybackList
{    
    ObservableCollection<MediaItem> Items { get; set; }
    int CurrentIndex { get; set; }
    MediaItem? GetCurrentItem();
    MediaItem? GetNextItem();
    MediaItem? GetPreviousItem();
    void AddItems(IEnumerable<string> mediaFiles);
    void RemoveItem(MediaItem? itemToRemove);
}
