namespace WinMix.Models;

public class PlaybackList
{        
    public ObservableCollection<MediaItem> Items { get; } = new();
    
    public PlaybackList()
    {
        Items = new();        
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
    
}            
