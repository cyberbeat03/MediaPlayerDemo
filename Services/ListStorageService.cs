using System.Xml.Linq;

namespace WinMix.Services;

public class ListStorageService
{
    private readonly string _playlistLocation = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    public async Task<IEnumerable<string>> LoadPlaylistAsync(string filePath)
    {
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

    public async Task SavePlaylistAsync(string playlistFile, IEnumerable<string> fileList)
    {        
        try
        {
string fullPath          =Path.Combine(_playlistLocation, $"{playlistFile}.wmx");
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);
            await File.WriteAllTextAsync(fullPath, builder.ToString());            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save playlist {playlistFile}: {ex.Message}");            
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
