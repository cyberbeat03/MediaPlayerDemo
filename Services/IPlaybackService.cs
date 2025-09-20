namespace WinMix.Services;

public interface IPlaybackService
{
    ObservableCollection<MediaItem> Items { get; }
    int CurrentIndex { get; set; }    
    string Name { get; set; }

    void AddItem(MediaItem item);    
    MediaItem? GetCurrentItem();
    MediaItem? GetNextItem();
    MediaItem? GetPreviousItem();
    void RemoveItem(MediaItem? itemToRemove);
    IEnumerable<string> GetFilePaths();
}