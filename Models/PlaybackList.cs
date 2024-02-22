namespace WinMix.Models;

public class PlaybackList
{    
    private int _currentIndex;

    public ObservableCollection<MediaItem> Items { get; } = new();

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
        Items = new();
        CurrentIndex = 0;
    }

    public void AddFiles(IList<string> mediaFiles)
    {
        if (mediaFiles.Count > 0)
        {            
            foreach (string mediaFile in mediaFiles)
            {
                MediaItem item = new(new FileInfo(mediaFile));
                
                                Items.Add(item);
            }            
        }
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
