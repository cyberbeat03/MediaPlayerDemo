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

    public MediaItem? CurrentItem
    {
        get => (Items.Count > 0) ? Items[_currentIndex] : null;
    }

    public PlaybackList()
    {
        _items = new();
        _currentIndex = 0;
    }    
    
    public MediaItem? GetPreviousItem()
    {
        if (CurrentIndex > 0)
        {
            CurrentIndex--;
        }        
        return CurrentItem;
    }            

    public MediaItem? GetNextItem()
    {
        if (CurrentIndex < Items.Count - 1)
        {
            CurrentIndex++;
        }        
        return CurrentItem;
    }

}            
