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
            var items = await JsonSerializer.DeserializeAsync<List<MediaItem>>(fs);
            return items ?? Enumerable.Empty<MediaItem>();
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

            await using FileStream fs = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(fs, fileList);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Could not save the playlist: {e.Message}");
        }
    }

}
