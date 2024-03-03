using WinMix.Services;

namespace WinMix.ViewModels;

public partial class PlaylistViewModel : MainViewModelBase
{
    [ObservableProperty] MediaItem? _selectedItem;
    [ObservableProperty] ObservableCollection<MediaItem> _items;
    
    public PlaylistViewModel()
    {
        Items = new();
    }    

    async Task LoadAsync(string fileName)
    {
        ListDataService listService = new();
        IList<string> files = await listService.LoadDataAsync(fileName);
        AddFiles(files);
    }

  async Task SaveAsync(string fileName)
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

    void AddFiles(IList<string> mediaFiles)
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

    [RelayCommand]
    void AddMediaFiles()
    {
        FileOpenService fileService = new();

        IList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count != 0)
            AddFiles(pickedFiles);
    }
    
    [RelayCommand]
    void RemoveItem()
    {
        MediaItem? itemToRemove = SelectedItem;

        if (itemToRemove is not null)        
          Items.Remove(itemToRemove);            
        }    

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = CurrentMediaList.Items.IndexOf(SelectedItem);
            if (currentPosition > 0)
                CurrentMediaList.Items.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is not null)
        {
            int currentPosition = CurrentMediaList.Items.IndexOf(SelectedItem);
            if (currentPosition < CurrentMediaList.Items.Count - 1)
                CurrentMediaList.Items.Move(currentPosition, currentPosition + 1);
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
        if (CurrentMediaList.Items.Count > 0)
        {
            List<string> filePaths = new();

            foreach (var item in CurrentMediaList.Items)
            {
                filePaths.Add(item.MediaPath);
            }

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