using System.Xml.Linq;
using System.Text.Json;

namespace WinMix.Services;

public class PlaylistService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    bool IsLocationValid
    {
        get
        {
            if (!Directory.Exists(_playlistLocation))
                Directory.CreateDirectory(_playlistLocation);

            return true;
        }
    }    

    public async Task<IReadOnlyList<string>> LoadM3UAsync(string playlistFilePath)
    {                
        if (!IsLocationValid) throw new IOException("Could not create or access playlist location.");

        var fullPath = Path.Combine(_playlistLocation, playlistFilePath);
        var outputList = new List<string>();

        var lines = await File.ReadAllLinesAsync(fullPath);

                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());            
        
            
        return outputList;
    }

    public async Task<bool> SaveToM3UAsync(string playlistName, IEnumerable<string> fileList)
    {
        if (!IsLocationValid) throw new IOException("Could not create or access playlist location.");
        var fullPath = Path.Combine(_playlistLocation, playlistName);
        
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);

            await File.WriteAllTextAsync(fullPath, builder.ToString());
            return true;        
    }

    public async Task<ObservableCollection<MediaItem>> LoadAsync(string playlistFilePath)
    {        
        

        var fullPath = Path.Combine(_playlistLocation, playlistFilePath);

        await using var stream = File.OpenRead(fullPath);
        return await JsonSerializer.DeserializeAsync < ObservableCollection<MediaItem>>(stream) ?? [];
    }

    public async Task<bool> SaveAsync(string playlistName, IEnumerable<MediaItem> items)
    {
        if (!IsLocationValid) throw new IOException("Could not create or access playlist.");            

            var fullPath = Path.Combine(_playlistLocation, playlistName);
            await using var stream = File.Create(fullPath);
            await JsonSerializer.SerializeAsync(stream, items);

            return true;        
    }

    public IReadOnlyList<string> ConvertWplToM3u(string wplPath)
    {
        var outputList = new List<string>();        

        if (!File.Exists(wplPath)) throw new FileNotFoundException("The file was not found.");

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
