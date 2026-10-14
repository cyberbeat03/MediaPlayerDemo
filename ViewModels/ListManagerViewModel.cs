using System.IO;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinMix.ViewModels;

public partial class ListManagerViewModel : ObservableObject
{
    public string PlaylistFolder { get; }
    public ObservableCollection<string> PlaylistFiles { get; } = new();

    [ObservableProperty]
    string? _selectedPlaylist;

    public ListManagerViewModel()
    {
        PlaylistFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "playlists");
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
        if (string.IsNullOrWhiteSpace(SelectedPlaylist)) return;

        var filePath = Path.Combine(PlaylistFolder, SelectedPlaylist);
        if (File.Exists(filePath))
        {
            if (ConfirmDelete() == MessageBoxResult.Yes)
            {
                File.Delete(filePath);
                LoadPlaylists();
            }
        }
    }

    public string? GetSelectedPlaylistPath()
    {
        if (string.IsNullOrWhiteSpace(SelectedPlaylist)) return null;
        return Path.Combine(PlaylistFolder, SelectedPlaylist);
    }

    [RelayCommand]
    public void Open()
    {
        RequestClose?.Invoke(this, true);
    }

    [RelayCommand]
    public void Cancel()
    {
        RequestClose?.Invoke(this, false);
    }

    public event EventHandler<bool?>? RequestClose;

    MessageBoxResult ConfirmDelete() => MessageBox.Show("Are you sure you want to permanently delete the selected playlist?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
}
