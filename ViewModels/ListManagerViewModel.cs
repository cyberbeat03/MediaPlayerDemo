namespace WinMix.ViewModels;

public partial class ListManagerViewModel : BaseViewModel
{
    [ObservableProperty] MediaItem? _selectedItem;
    PlaybackList _playlist;

    public ObservableCollection<MediaItem> MediaItems
    {
        get => _playlist.Items;
    }

    public ListManagerViewModel(PlaybackList playbackList)
    {
        _playlist = playbackList;
    }

    async Task LoadPlaylistAsync(string fileName)
    {
        PlaylistService listService = new();

        IReadOnlyList<string> files = await listService.LoadAsync(fileName);

        if (files.Count > 0)
        {
            MediaItems.Clear();
            _playlist.AddItems(files);
        }
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

    [RelayCommand]
    void PickMediaFiles()
    {
        FileOpenService fileService = new();

        IReadOnlyList<string> pickedFiles = fileService.PickMediaFiles();

        if (pickedFiles.Count > 0)
            _playlist.AddItems(pickedFiles);
    }

    [RelayCommand]
    void RemoveItem()
    {
        if (SelectedItem is MediaItem item)
            _playlist.RemoveItem(item);
    }

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);
            if (currentPosition > 0)
                MediaItems.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = MediaItems.IndexOf(item);

            if (currentPosition < MediaItems.Count - 1)
                MediaItems.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        if (SelectedItem is MediaItem item)
        {
            ClipBoardService clipboard = new();
            clipboard.Copy(item.FullPath);
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

        if (returnedFiles.Count > 0)
            _playlist.AddItems(returnedFiles);
        else
            MessageBox.Show("Could not paste files from the clipboard.");
    }

}
