using System.Linq.Expressions;

namespace WinMix.Services;

public class PlaylistService
{
    readonly string _playlistFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
    "playlists");

    public async Task<IReadOnlyList<string>> LoadAsync(string fileToLoad)
    {
        List<string> outputList = new List<string>();                

        try
        {
            IReadOnlyList<string> lines = await File.ReadAllLinesAsync(fileToLoad);

            foreach (string line in lines)            
                if (!String.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                    outputList.Add(line.Trim());            
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading the playlist file {fileToLoad}{Environment.NewLine}{ex.Message}");
        }    

        return outputList;
        }

    public async Task<bool> SaveAsync(string fileToSave, IReadOnlyList<string> fileList)
    {
        try
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();

            foreach (var filePath in fileList)
                builder.AppendLine(filePath);

            await File.WriteAllTextAsync(fileToSave, builder.ToString());
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);            
            return false;
        }
    }

}
