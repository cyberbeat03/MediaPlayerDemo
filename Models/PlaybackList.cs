namespace WinMix.Models;

public class PlaybackList
{
    public ObservableCollection<MediaItem> Items { get; }
    public int CurrentIndex { get; set; }
    public string Name { get; set; }

    private bool IsIndexValid(int index) =>
        index >= 0 && index < Items.Count;

    public PlaybackList()
    {
        Items = new();
        CurrentIndex = -1;
        Name = "New Playlist";
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

    public IEnumerable<string> GetFiles()
    {
        List<string> pathList = new();

        foreach (var item in Items)
            pathList.Add(item.FullPath);

        return pathList;
    }        

    public void AddFiles(IEnumerable<string> filePaths)
    {
        foreach (var path in filePaths)
        {
            if (File.Exists(path))
            Items.Add(new MediaItem(new FileInfo(path)));
        }

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
