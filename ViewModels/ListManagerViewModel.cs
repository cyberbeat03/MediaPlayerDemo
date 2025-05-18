namespace WinMix.ViewModels;

public partial class ListManagerViewModel : BaseViewModel
{
    [ObservableProperty] MediaItem? _selectedItem;
    [ObservableProperty] ObservableCollection<MediaItem> _mediaItems;    

    public ListManagerViewModel(PlaybackList playlist)
    {
        MediaItems = playlist.Items;
        SelectedItem = playlist.CurrentItem;
    }    

    async Task LoadPlaylistAsync(string fileName)
    {
        PlaylistService listService = new();
        
        IReadOnlyList<string> files = await listService.LoadAsync(fileName);
        AddFiles(files);
    }

  async Task SavePlaylistAsync(string fileName)
    {        
        if (MediaItems.Count > 0)
        {            
            List<string> filePaths = new();

            foreach (var item in MediaItems)
                filePaths.Add(item.FullPath);

            PlaylistService listService = new();
            await listService.SaveAsync(fileName, filePaths);
        }
    }
    
    void AddFiles(IReadOnlyList<string> mediaFiles)
    {        
            foreach (string mediaFile in mediaFiles)            
                MediaItems.Add(new MediaItem(new FileInfo(mediaFile)));                    
    }

    [RelayCommand]
    void PickMediaFiles()
    {
        FileOpenService fileService = new();

        IReadOnlyList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count > 0)
            AddFiles(pickedFiles);
    }
    
    [RelayCommand]
    void RemoveItem()
    {        
        if (SelectedItem is not null)        
          MediaItems.Remove(SelectedItem);            
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
            clipboard.Copy(SelectedItem.FullPath);
        }
    }

    [RelayCommand]
    void CopyAllItems()
        {
            if (MediaItems.Count > 0)
            {
                List<string> filePaths = new();

                foreach (var item in MediaItems)
                    filePaths.Add(item.FullPath);

                ClipBoardService clipboard = new();
                clipboard.CopyAll(filePaths);
            }
        }    

    [RelayCommand]
    void PasteItems()
    {
        ClipBoardService clipBoard = new();
        IReadOnlyList<string>? returnedFiles = clipBoard.Paste();
        if (returnedFiles is not null)
          AddFiles(returnedFiles);
    }

}
