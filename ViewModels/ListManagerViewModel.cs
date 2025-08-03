namespace WinMix.ViewModels;

public partial class ListManagerViewModel : ObservableObject
{
    [ObservableProperty] MediaItem? _selectedItem;
    [ObservableProperty] string _playlistName;
    [ObservableProperty] string _listTitle;
    public ObservableCollection<MediaItem> MediaItems { get; set; }
    public ListManagerViewModel(PlaybackList playbackList)
    {
        MediaItems = playbackList.Items;
        PlaylistName = playbackList.Name;
        ListTitle = $"Playlist: {playbackList.Name} - List Manager";
    }

    [RelayCommand]
    void PickMedia()
    {
        var pickedFiles = new FileOpenService().PickMediaFiles();
        foreach (var file in pickedFiles)
            MediaItems.Add(MediaItem.FromFile(file));
    }

    [RelayCommand]
    void RemoveSelectedItem()
    {
        if (SelectedItem is MediaItem item)
            MediaItems.Remove(item);
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
            new ClipBoardService().Copy(item.FullPath);
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = new ClipBoardService().Paste();
        foreach (var item in pastedItems)
            MediaItems.Add(MediaItem.FromFile(item));
    }

    public IEnumerable<string> GetFiles()
    {
        List<string> pathList = new();

        foreach (var item in MediaItems)
            pathList.Add(item.FullPath);

        return pathList;
    }

    [RelayCommand]
    async Task SaveList()
    {
        await new ListStorageService().SavePlaylistAsync(PlaylistName, GetFiles());
    }

    [RelayCommand]
    async Task LoadList()
    {
        string playlistFile = new FileOpenService().PickPlaylistFile();
        if (!string.IsNullOrEmpty(playlistFile))
        {
            MediaItems.Clear();
            PlaylistName = Path.GetFileNameWithoutExtension(playlistFile);
            ListTitle = $"Playlist: {PlaylistName} - List Manager";
            var files = await new ListStorageService().LoadPlaylistAsync(playlistFile);
            foreach (var file in files)
                MediaItems.Add(MediaItem.FromFile(file));
        }
    }


    [RelayCommand]
    void NewList()
    {
        var inputDialog = new InputTextDialog();
if (inputDialog.ShowDialog() == true)
        {
            string input = inputDialog.Response;           
            
            MediaItems.Clear();
                PlaylistName = input;
            ListTitle = $"Playlist: {input} - List Manager";
        }
    }

}
