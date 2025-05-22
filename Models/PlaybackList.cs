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
        _currentIndex = -1;
    }

    public MediaItem? CurrentItem
    {
        get
        {
if (CurrentIndex >= 0 && CurrentIndex < Items.Count)
            {
                return Items[CurrentIndex];
        }

return null;
    }
    }

public MediaItem? PreviousItem
    {
        get
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex--;
            }

            return CurrentItem;
        }
    }

    public MediaItem? NextItem
    {
        get
        {
            if (CurrentIndex < Items.Count - 1)
            {
                CurrentIndex++;
            }
            return CurrentItem;
        }
    }
}            