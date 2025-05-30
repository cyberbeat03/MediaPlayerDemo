namespace WinMix.Models;

public interface IPlaybackList
{
    int CurrentIndex { get; set; }
    MediaItem? CurrentItem { get; }
    ObservableCollection<MediaItem> Items { get; set; }
    MediaItem? NextItem { get; }
    MediaItem? PreviousItem { get; }

    void AddFiles(IEnumerable<string> mediaFiles);
}
