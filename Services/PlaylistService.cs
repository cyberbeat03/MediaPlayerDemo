using System.Xml.Linq;

namespace WinMix.Services;

public class PlaylistService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    public async Task<IEnumerable<string>> LoadAsync(string playlistFile)
    {
        if (Path.GetExtension(playlistFile).ToLower() == ".wpl")
            return ConvertWplToM3u(playlistFile);

        var outputList = new List<string>();

var fullPath = Path.Combine(_playlistLocation, playlistFile);
        var lines = await File.ReadAllLinesAsync(fullPath);
        foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());                    
            
        return outputList;
    }

    public async Task SaveAsync(string playlistName, IEnumerable<string> fileList)
    {
        if (Directory.Exists(_playlistLocation) == false)
            Directory.CreateDirectory(_playlistLocation);
        var fullPath = Path.Combine(_playlistLocation, $"{playlistName}.m3u8");

            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);

        using var fileStream = File.Create(fullPath);        
            using var writer = new StreamWriter(fileStream);
            await writer.WriteAsync(builder.ToString());            
        }

        
    
    public IEnumerable<string> ConvertWplToM3u(string wplPath)
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
