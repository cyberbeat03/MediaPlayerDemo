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
    
    public string Name { get; set; }

    private bool IsIndexValid(int index) =>
        index >= 0 && index < Items.Count;

    public PlaybackList()
    {
        Items = new();
        CurrentIndex = -1;
        Name = "Untitled Playlist";
    }            
    
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

    public void AddItem(MediaItem item)
    {                           
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
