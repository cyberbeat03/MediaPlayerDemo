using System.Text.Json;
using System.Xml.Linq;

namespace WinMix.Services;

public class StorageService
{
    readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");        

    public async Task<IEnumerable<MediaItem>> LoadPlaylistAsync()
    {
        var fullPath = Path.Combine(_playlistLocation, "WinMix.json");
        if (!File.Exists(fullPath)) return Enumerable.Empty<MediaItem>();

        try
        {
await using         FileStream fs = File.OpenRead(fullPath);
        return await JsonSerializer.DeserializeAsync<IEnumerable<MediaItem>>(fs);
    }
        catch (Exception e)
        {
            MessageBox.Show($"Could not restore the last playlist: {e.Message}");
            return Enumerable.Empty<MediaItem>();
        }
    }

    public async Task SavePlaylistAsync(IEnumerable<MediaItem> fileList)
    {
        var fullPath = Path.Combine(_playlistLocation, "WinMix.json");

        try
        {
            await using FileStream fs = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(fs, fileList);
        }
        catch (Exception e)
        {
            MessageBox.Show($"Could not save the playlist: {e.Message}");
        }
        }

    public IEnumerable<string> ConvertWplToM3u(string wplPath)
    {
        XDocument doc = XDocument.Load(wplPath);

        string basePath = Path.GetDirectoryName(wplPath)!;

        var mediaElements = doc.Descendants("media");

        foreach (var media in mediaElements)
        {
            string? src = media.Attribute("src")?.Value;
            if (!string.IsNullOrWhiteSpace(src))
            {
                string fullPath = Path.GetFullPath(Path.Combine(basePath, src));
                yield return fullPath;
            }
        }
    }

}
