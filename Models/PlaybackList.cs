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
            if (Items.Count == 0)
                return null;

            CurrentIndex = (CurrentIndex - 1 + Items.Count) % Items.Count;

            return CurrentItem;
        }
    }

    public MediaItem? NextItem
    {
        get
        {
            if (Items.Count == 0)
                return null;

            CurrentIndex = (CurrentIndex + 1) % Items.Count;
            return CurrentItem;
        }
    }

    public void AddFiles(IEnumerable<string> mediaFiles)
    {
        foreach (string mediaFile in mediaFiles)
            Items.Add(new MediaItem(new FileInfo(mediaFile)));

        if (CurrentIndex == -1 && Items.Count > 0)
            CurrentIndex = 0;
    }

}
