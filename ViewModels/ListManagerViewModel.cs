namespace WinMix.ViewModels;

public partial class ListManagerViewModel : ObservableObject
{
    public string PlaylistFolder { get; }
    public ObservableCollection<string> PlaylistFiles { get; } = new ObservableCollection<string>();
    public string? SelectedPlaylist { get; set; }

    public string ListTitle { get; set; }

    public ListManagerViewModel()
    {
        PlaylistFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "playlists");
        ListTitle = "Playlists";
        LoadPlaylists();
    }

    void LoadPlaylists()
    {
        PlaylistFiles.Clear();
        if (!Directory.Exists(PlaylistFolder)) return;

        var files = Directory.EnumerateFiles(PlaylistFolder, "*.wmx")
                             .Select(Path.GetFileName);

        foreach (var file in files)
            PlaylistFiles.Add(file);
    }

    [RelayCommand]
    public void DeletePlaylist()
    {
        if (SelectedPlaylist == null) return;
        
            var filePath = Path.Combine(PlaylistFolder, $"{SelectedPlaylist}.wmx");
            if (File.Exists(filePath))
            {
                if (ConfirmDelete() == MessageBoxResult.Yes)
                {
                    File.Delete(filePath);
                    LoadPlaylists();
                }                
            }        
    }

MessageBoxResult     ConfirmDelete() => MessageBox.Show("Are you sure you want to permanently delete the selected playlist?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);    
}