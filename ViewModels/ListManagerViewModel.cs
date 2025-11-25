using System.Threading.Tasks;

namespace WinMix.ViewModels;

public partial class ListManagerViewModel : ObservableObject
{
    [ObservableProperty] MediaItem? _selectedItem;    
    [ObservableProperty] string _listTitle;    
    IPlaybackService _playlist;

    public ObservableCollection<MediaItem> MediaItems => _playlist.Items;

    public ListManagerViewModel(IPlaybackService playbackService)
    {
        _playlist = playbackService;                
        ListTitle = $"Playlist: {_playlist.Name} - List Manager";
    }

    [RelayCommand]
    void PickMedia()
    {
        var pickedFiles = new FileOpenService().PickMediaFiles();
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
            new ClipBoardService().Copy(item.FullPath);
    }

    [RelayCommand]
    void PasteItems()
    {
        var pastedItems = new ClipBoardService().Paste();
        foreach (var item in pastedItems)
            _playlist.AddItem(MediaItem.FromFile(item));
    }
    
    [RelayCommand]
    async Task SaveList()
    {
await         new StorageService().SavePlaylistAsync(_playlist.Items);
    }

    [RelayCommand]
async     Task LoadList()
    {
        string playlistFile = new FileOpenService().PickPlaylistFile();
        if (!string.IsNullOrEmpty(playlistFile))
        {
            _playlist.Items.Clear();

            _playlist.Name = Path.GetFileNameWithoutExtension(playlistFile);
            ListTitle = $"Playlist: {_playlist.Name} - List Manager";
            var items = await new StorageService().LoadPlaylistAsync();
            foreach (var item in items)
                _playlist.AddItem(item);
        }
    }

    [RelayCommand]
    async Task CreateNewList()
    {
        var inputDialog = new InputTextDialog();
if (inputDialog.ShowDialog() == true)
        {
            string input = inputDialog.Response;           
            
            _playlist.Items.Clear();
            _playlist.Name = "input";
            ListTitle = $"Playlist: {_playlist.Name} - List Manager";
await             new StorageService().SavePlaylistAsync(_playlist.Items);
        }
    }

[RelayCommand]
    void StartPlayback()
    {

    }

}
