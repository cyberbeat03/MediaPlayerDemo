namespace WinMix.ViewModels;

public class ListManagerViewModel : ObservableObject
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
                             .Select(Path.GetFileNameWithoutExtension);

        foreach (var file in files)
            PlaylistFiles.Add(file);
    }
}
