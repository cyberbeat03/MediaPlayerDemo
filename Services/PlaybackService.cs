namespace WinMix.Services;

public class PlaybackService : IPlaybackService
{
    public ObservableCollection<MediaItem> Items { get; } = new();
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

public void MoveUp(MediaItem? mediaItem)
    {
        if (mediaItem is null || !Items.Contains(mediaItem)) return;
        
        int currentPosition = Items.IndexOf(mediaItem);
            if (currentPosition > 0)
                Items.Move(currentPosition, currentPosition - 1);        
    }

    public void MoveDown(MediaItem? mediaItem)
{
    if (mediaItem is null || !Items.Contains(mediaItem)) return;

                int currentPosition = Items.IndexOf(mediaItem);

        if (currentPosition < Items.Count - 1)
            Items.Move(currentPosition, currentPosition + 1);    
}

public IEnumerable<string> GetFilePaths()
    {        
        var paths = Items.Select(i => i.FullPath).ToList();
        foreach (var path in paths)
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                yield return path;
        }
    }

    public void AddItem(MediaItem item)
    {
        if (item is null)
            return;

        if (!Items.Contains(item))
        Items.Add(item);

        if (CurrentIndex <= -1 && Items.Count > 0)
            CurrentIndex = 0;
    }

    public void RemoveItem(MediaItem? itemToRemove)
    {
        if (itemToRemove is null)
            return;

        var removedIndex = Items.IndexOf(itemToRemove);
        if (removedIndex < 0)
            return;

        Items.RemoveAt(removedIndex);

        if (Items.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        if (removedIndex < CurrentIndex)
        {
            CurrentIndex--;
            return;
        }

        if (CurrentIndex >= Items.Count)
            CurrentIndex = Items.Count - 1;
    }
}
