using System.Xml.Linq;

namespace WinMix.Services;

public class ListStorageService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    public async Task<IEnumerable<string>> LoadPlaylistAsync(string filePath)
    {
if (Path.GetExtension(filePath).ToLower() ==".wpl")
            return ConvertWplToM3u(filePath);

        var outputList = new List<string>();
        try
        {
                var lines = await File.ReadAllLinesAsync(filePath);
                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load {filePath}: {ex.Message}");
        }

        return outputList;
    }

    public async Task SavePlaylistAsync(string fileName, IEnumerable<string> fileList)
    {        
        try
        {
        var fullPath = Path.Combine(_playlistLocation, fileName);
        
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);
            await File.WriteAllTextAsync(fullPath, builder.ToString());            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save playlist: {ex.Message}");            
        }    
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
