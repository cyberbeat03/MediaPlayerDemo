namespace WinMix.ViewModels;

public partial class ListManagerViewModel : BaseViewModel
{
    [ObservableProperty] string _playlistName = string.Empty;
    [ObservableProperty] MediaItem? _selectedItem = null;
    public ObservableCollection<MediaItem> MediaItems { get; set; } = new();

    public ListManagerViewModel(PlaybackList playbackList)
    {
        MediaItems = playbackList.Items;
    }

    [RelayCommand]
    void PickMedia()
    {
        var pickedFiles = new FileOpenService().PickMediaFiles();
        if (pickedFiles.Count() > 0)
        {
            foreach (var file in pickedFiles)
                MediaItems.Add(MediaItem.FromFile(file));
        }
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

    [RelayCommand]
    void NewPlaylist()
    {
        var inputDialog = new InputTextDialog();

        if (inputDialog.ShowDialog() == true)
        {
            MediaItems.Clear();
            PlaylistName = inputDialog.Response;            
        }
    }


}
