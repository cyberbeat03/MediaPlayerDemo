using System.Diagnostics;

namespace WinMix.Models;

public class PlaybackList
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
            Debug.WriteLine($"From previous method,  CurrentIndex is now {CurrentIndex} of {Items.Count - 1}");
            return Items[CurrentIndex];
        }

        return null;
    }

    public MediaItem? GetNextItem()
{
        if (IsIndexValid(CurrentIndex + 1))
{
   CurrentIndex++;
            Debug.WriteLine($"From next method,  CurrentIndex is now {CurrentIndex} of {Items.Count - 1}");
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
        if (itemToRemove is null) return;

        Items.Remove(itemToRemove);
        Debug.WriteLine($"After removing,  CurrentIndex is now  {CurrentIndex}  of  {Items.Count - 1}");

        if (CurrentIndex >= Items.Count)
        {
            CurrentIndex = Items.Count - 1;
            Debug.WriteLine($"CurrentIndex was moved back to {CurrentIndex} of {Items.Count - 1}");
        }
    }

}
