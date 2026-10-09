using System.Text.Json;

namespace WinMix.Services;

public class StorageService : IStorageService
{
    readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    public async Task<IEnumerable<MediaItem>> LoadPlaylistAsync(string wmxFileName)
    {
        var fullPath = Path.Combine(_playlistLocation, wmxFileName);
        if (!File.Exists(fullPath)) return Enumerable.Empty<MediaItem>();

        try
        {
            await using FileStream fs = File.OpenRead(fullPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var playlist = await JsonSerializer.DeserializeAsync<Playlist>(fs, options);
            return (playlist?.Items == null) ? Enumerable.Empty<MediaItem>() : playlist.Items;
        }
        catch (Exception)
        {
            return Enumerable.Empty<MediaItem>();
        }
    }

    public async Task SavePlaylistAsync(string wmxFileName, IEnumerable<MediaItem> fileList)
    {
        var fullPath = Path.Combine(_playlistLocation, wmxFileName);

        try
        {
            if (!Directory.Exists(_playlistLocation)) Directory.CreateDirectory(_playlistLocation);

            var playlist = new Playlist
            {
                Version = 1,
                Name = Path.GetFileNameWithoutExtension(wmxFileName),
                Items = fileList.ToList()
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            await using FileStream fs = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(fs, playlist, options);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Could not save the playlist: {e.Message}");
        }
    }

}
