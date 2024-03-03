namespace WinMix.Models;

public class PlaybackList
{
    private ObservableCollection<MediaItem> _items;
    private int _currentIndex;

    public ObservableCollection<MediaItem> Items
    {
        get => _items;
        set
        {
            if (_items != value)
            {
                _items = value;
            }
        }
    }

    public int CurrentIndex
    {
        get => _currentIndex;
        private set
        {
            if (_currentIndex != value)
            {
                _currentIndex = value;
            }
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
    
    public void SetCurrentIndex(int index)
    {
        int maxValue = Items.Count;

        if (index < 0)
        {
            CurrentIndex = 0;
    }
        else if (index > maxValue)
        {
            CurrentIndex = maxValue - 1;
        }
        else
        {
CurrentIndex= index;
        }
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
