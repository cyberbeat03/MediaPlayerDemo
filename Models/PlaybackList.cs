namespace WinMix.Models;

public class PlaybackList : IPlaybackList
{
    private int _currentIndex;
    private ObservableCollection<MediaItem> _items;

    public ObservableCollection<MediaItem> Items
    {
        get => _items;
        set
        {
            if (_items != value)
                _items = value;
        }
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (_currentIndex != value)
                _currentIndex = value;
        }
    }    

    public PlaybackList()
    {
        _items = new();
        _currentIndex = 0;
    }

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

    public void AddItems(IEnumerable<string> mediaFiles)
    {
        foreach (string mediaFile in mediaFiles)
            Items.Add(new MediaItem(new FileInfo(mediaFile)));        
    }

    public void RemoveItem(MediaItem? itemToRemove)
{                            
        Items.Remove(itemToRemove);

        if (CurrentIndex >= Items.Count)
            CurrentIndex = Items.Count - 1;
    }

}
