using System.Xml.Linq;

namespace WinMix.Services;

public class PlaylistService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    bool ValidatePlaylistLocation()
    {
        try
        {
            if (!Directory.Exists(_playlistLocation))
                Directory.CreateDirectory(_playlistLocation);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not create or access playlist location: {ex.Message}");
            return false;
        }

        return true;
    }

    public async Task<IReadOnlyList<string>> LoadM3U8Async(string playlistName)
    {
        var outputList = new List<string>();

        if (!ValidatePlaylistLocation()) return outputList;

        var fullPath = Path.Combine(_playlistLocation, playlistName);
        try
        {
            if (File.Exists(fullPath))
            {
                var lines = await File.ReadAllLinesAsync(fullPath);
                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load {playlistName}: {ex.Message}");
        }

        return outputList;
    }

    public async Task<bool> SaveToM3U8Async(string fileName, IEnumerable<string> fileList)
    {
        if (!ValidatePlaylistLocation()) return false;
        var fullPath = Path.Combine(_playlistLocation, fileName);
        try
        {
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);
            await File.WriteAllTextAsync(fullPath, builder.ToString());
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save playlist: {ex.Message}");
            return false;
        }
    }

    public async Task<IReadOnlyList<string>> ConvertWplToM3u8Async(string wplPath)
    {
        var outputList = new List<string>();
        if (!ValidatePlaylistLocation()) return outputList;

        if (!File.Exists(wplPath)) return outputList;
        
            XDocument doc = XDocument.Load(wplPath);

            string basePath = Path.GetDirectoryName(wplPath)!;

            var mediaElements = doc.Descendants("media");

        foreach (var media in mediaElements)
        {
            string? src = media.Attribute("src")?.Value;
            if (!string.IsNullOrWhiteSpace(src))
            {                
                string fullPath = Path.GetFullPath(Path.Combine(basePath, src));
                outputList.Add(fullPath);
            }
        }

        return outputList;
    }

}
