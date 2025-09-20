namespace WinMix.Services;

public class PlaybackService : IPlaybackService
{    
    public ObservableCollection<MediaItem> Items { get; } = new ObservableCollection<MediaItem>();
    public int CurrentIndex { get; set; } = -1;
    public string Name { get; set; } = "Untitled Playlist";

    private bool IsIndexValid(int index) =>
        index >= 0 && index < Items.Count;    

    public MediaItem? GetCurrentItem() =>
        IsIndexValid(CurrentIndex) ? Items[CurrentIndex] : null;

    public MediaItem? GetPreviousItem()
    {
        if (IsIndexValid(CurrentIndex - 1))
        {
            CurrentIndex--;
            return Items[CurrentIndex];
        }

        return null;
    }

    public MediaItem? GetNextItem()
    {
        if (IsIndexValid(CurrentIndex + 1))
        {
            CurrentIndex++;
            return Items[CurrentIndex];
        }

        return null;
    }

    public IEnumerable<string> GetFilePaths()
    {
        var files = Items.Select(item => item.FullPath);
        var validFiles = new List<string>();

        foreach (var file in files)
        {
            if (File.Exists(file))
                validFiles.Add(file);
        }

        return validFiles;
    }   

    public void AddItem(MediaItem item)
    {
        if (item is null) return;
        
        Items.Add(item);

        if (CurrentIndex <= -1 && Items.Count > 0)
            CurrentIndex = 0;
    }

    public void RemoveItem(MediaItem? itemToRemove)
    {
        if (itemToRemove is null) return;

        Items.Remove(itemToRemove);

        if (CurrentIndex >= Items.Count)
            CurrentIndex = Items.Count - 1;
    }

}
