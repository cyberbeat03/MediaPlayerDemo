using WinMix.Services;

namespace WinMix.ViewModels;

public partial class PlaylistViewModel : MainViewModelBase
{
    [ObservableProperty] MediaItem? _selectedItem;
    [ObservableProperty] ObservableCollection<MediaItem> _mediaItems = new();
    ListDataService _listService = new();

    public PlaylistViewModel(ObservableCollection<MediaItem> mediaItems)
    {
        if (mediaItems.Count > 0)
            MediaItems = mediaItems;
        else
            FindMediaFiles();        
    }    

    async Task LoadAsync(string fileName)
    {        
        IList<string> files = await _listService.LoadDataAsync(fileName);
        AddFiles(files);
    }

  async Task SaveAsync(string fileName)
    {        
        if (MediaItems.Count > 0)
        {            
            List<string> filePaths = new();

            foreach (var item in MediaItems)
                filePaths.Add(item.MediaPath);

            await _listService.SaveDataAsync(fileName, filePaths);
        }
    }
    
    void AddFiles(IList<string> mediaFiles)
    {        
            foreach (string mediaFile in mediaFiles)            
                MediaItems.Add(new MediaItem(new FileInfo(mediaFile)));                    
    }

    void FindMediaFiles()
    {
        IList<FileInfo> mediaFiles = _listService.SearchForMedia();

        foreach (FileInfo mediaFile in mediaFiles)
            MediaItems.Add(new MediaItem(mediaFile));
    }
    
    public PlaybackList GetPlaybackList()
    {
        PlaybackList list = new();

        list.Items = MediaItems;
        if (SelectedItem is not null)
            list.CurrentIndex = MediaItems.IndexOf(SelectedItem);
        else
            list.CurrentIndex = 0;

return list;
    }
    
    [RelayCommand]
    void PickMediaFiles()
    {
        FileOpenService fileService = new();

        IList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count > 0)
            AddFiles(pickedFiles);
    }
    
    [RelayCommand]
    void RemoveItem()
    {
        MediaItem? itemToRemove = SelectedItem;

        if (itemToRemove is not null)        
          MediaItems.Remove(itemToRemove);            
        }    

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = MediaItems.IndexOf(SelectedItem);
            if (currentPosition > 0)
                MediaItems.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = MediaItems.IndexOf(SelectedItem);            

             if (currentPosition < MediaItems.Count - 1)
MediaItems.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        if (SelectedItem is not null)
        {
            ClipBoardService clipboard = new();
            clipboard.Copy(SelectedItem.MediaPath);
        }
    }

    [RelayCommand]
    void CopyAllItems()
    {
        if (MediaItems.Count > 0)
        {
            List<string> filePaths = new();

            foreach (var item in MediaItems)            
                filePaths.Add(item.MediaPath);            

            ClipBoardService clipboard = new();
            clipboard.CopyAll(filePaths);
        }
    }

    [RelayCommand]
    void PasteItems()
    {
        ClipBoardService clipBoard = new();
        IList<string>? returnedFiles = clipBoard.Paste();
        if (returnedFiles is not null)
          AddFiles(returnedFiles);
    }

}
