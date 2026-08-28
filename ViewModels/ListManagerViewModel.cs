namespace WinMix.ViewModels;

public partial class ListManagerViewModel : ObservableObject
{
    [ObservableProperty] MediaItem? _selectedItem;    
    [ObservableProperty] string _listTitle;    
    readonly IPlaybackService _playlist;
    readonly IFileOpenService _fileOpenService;
    readonly IStorageService _storageService;
    readonly IClipBoardService _clipBoardService;
    readonly IWindowDisplayService _windowDisplayService;

    public ObservableCollection<MediaItem> MediaItems => _playlist.Items;

    public ListManagerViewModel(
        IPlaybackService playbackService,
        IFileOpenService fileOpenService,
        IStorageService storageService,
        IClipBoardService clipBoardService,
        IWindowDisplayService? windowDisplayService = null)
    {
        _playlist = playbackService;
        _fileOpenService = fileOpenService;
        _storageService = storageService;
        _clipBoardService = clipBoardService;
        _windowDisplayService = windowDisplayService;
        ListTitle = $"{_playlist.Name} - List Manager";
    }

    [RelayCommand]
    void PickMedia()
    {
        var pickedFiles = _fileOpenService.PickMediaFiles();
        foreach (var file in pickedFiles)
            _playlist.AddItem(MediaItem.FromFile(file));
    }

    [RelayCommand]
    void RemoveSelectedItem()
    {
        if (SelectedItem is MediaItem item)
            _playlist.RemoveItem(item);
    }

    [RelayCommand]
    void MoveItemUp()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = _playlist.Items.IndexOf(item);
            if (currentPosition > 0)
                _playlist.Items.Move(currentPosition, currentPosition - 1);
        }
    }

    [RelayCommand]
    void MoveItemDown()
    {
        if (SelectedItem is MediaItem item)
        {
            int currentPosition = _playlist.Items.IndexOf(item);

            if (currentPosition < _playlist.Items.Count - 1)
                _playlist.Items.Move(currentPosition, currentPosition + 1);
        }
    }

    [RelayCommand]
    void CopyItem()
    {
        if (SelectedItem is MediaItem item)
            _clipBoardService.Copy(item.FullPath);
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = _clipBoardService.Paste();
        foreach (var item in pastedItems)
            _playlist.AddItem(MediaItem.FromFile(item));
    }
    
    [RelayCommand]
    async Task SaveList()
    {
        await _storageService.SavePlaylistAsync(_playlist.Name + ".wmx", _playlist.Items);
    }

    [RelayCommand]
    async Task LoadList()
    {
        string playlistFile = _fileOpenService.PickPlaylistFile();
        if (!string.IsNullOrEmpty(playlistFile))
        {
            _playlist.Items.Clear();

            _playlist.Name = Path.GetFileNameWithoutExtension(playlistFile);
            ListTitle = $"{_playlist.Name} - List Manager";
            var items = await _storageService.LoadPlaylistAsync(playlistFile);
            foreach (var item in items)
                _playlist.AddItem(item);
        }
    }

    [RelayCommand]
    async Task StartPlayback()
    {
        await SaveList();
        _windowDisplayService.ShowPlayer();
    }
}
