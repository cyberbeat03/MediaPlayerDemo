using WinMix.Services;

namespace WinMix.Models;

public class PlaybackList
{        
    public ObservableCollection<MediaItem> Items { get; } = new();
    
    public PlaybackList()
    {        
    }

    public async Task LoadAsync(string fileName)
    {
ListDataService listService = new();
IList<string> files = await listService.LoadDataAsync(fileName);
        AddFiles(files);
    }

    public async Task SaveAsync(string fileName)
    {                        
        List<string> filePaths = new();
        
        if (Items.Count > 0)
        {
            ListDataService listService = new();
            foreach (var item in Items)
                filePaths.Add(item.MediaName);
            await listService.SaveDataAsync(fileName, filePaths);
        }
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
