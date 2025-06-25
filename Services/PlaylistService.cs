using System.Xml.Linq;
using System.Text.Json;

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
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not create or access playlist location: {ex.Message}");
            return false;
        }        
    }
/*
    public async Task<IReadOnlyList<string>> LoadM3UAsync(string playlistFilePath)
    {
        var outputList = new List<string>();        
        
        try
        {
            if (File.Exists(playlistFilePath))
            {
                var lines = await File.ReadAllLinesAsync(playlistFilePath);
                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load playlist: {ex.Message}");
            return outputList;
        }

        return outputList;
    }

    public async Task<bool> SaveToM3UAsync(string playlistName, IEnumerable<string> fileList)
    {
        if (!ValidatePlaylistLocation()) return false;
        var fullPath = Path.Combine(_playlistLocation, playlistName);
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
*/

    public async Task<PlaybackList> LoadPlaybackListAsync(string playlistFilePath)
    {
        if (string.IsNullOrWhiteSpace(playlistFilePath) || !File.Exists(playlistFilePath))
            return new PlaybackList();

        try
        {
            await using var stream = File.OpenRead(playlistFilePath);
            var playlist = await JsonSerializer.DeserializeAsync<PlaybackList>(stream);
            return playlist ?? new PlaybackList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load list: {ex.Message}");
            return new PlaybackList();
        }
    }

    public async Task<bool> SavePlaybackListAsync(string playlistName, PlaybackList playbackList)
    {
        if (string.IsNullOrWhiteSpace(playlistName) || playbackList == null)
            return false;

        if (!ValidatePlaylistLocation()) return false;

        try
        {
            var fullPath = Path.Combine(_playlistLocation, playlistName);
            await using var stream = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(stream, playbackList);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save playback list: {ex.Message}");
            return false;
        }
    }

    public IReadOnlyList<string> ConvertWplToM3u(string wplPath)
    {
        var outputList = new List<string>();        

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
