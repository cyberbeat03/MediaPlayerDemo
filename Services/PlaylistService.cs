using System.Xml.Linq;
using System.Text.Json;

namespace WinMix.Services;

public class PlaylistService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");    

    public async Task<IReadOnlyList<string>> LoadAsync(string playlistFilePath)
    {
        if (Path.GetExtension(playlistFilePath).ToLower() == ".wpl")
            return ConvertWplToM3u(playlistFilePath);

        var outputList = new List<string>();

String fullPath = Path.Combine(_playlistLocation, playlistFilePath);
        var lines = await File.ReadAllLinesAsync(fullPath);
        foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());                    
            
        return outputList;
    }

    public async Task<bool> SaveAsync(string playlistName, IEnumerable<string> fileList)
    {
        if (Directory.Exists(_playlistLocation) == false)
            Directory.CreateDirectory(_playlistLocation);
        var fullPath = Path.Combine(_playlistLocation, $"{playlistName}.m3u8");
        
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);

            await File.WriteAllTextAsync(fullPath, builder.ToString());
            return true;        
    }
    
    public IReadOnlyList<string> ConvertWplToM3u(string wplPath)
    {
        var outputList = new List<string>();                

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
