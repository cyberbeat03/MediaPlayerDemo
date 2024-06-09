namespace WinMix.Services;

public class PlaylistService
{
    readonly string _playlistFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
    "playlists");

    public async Task<IList<string>> LoadDataAsync(string fileToLoad)
    {
        List<string> outputList = new();

        IReadOnlyList<string> lines = await File.ReadAllLinesAsync(fileToLoad);

            foreach (string line in lines)
            {
                if (!line.StartsWith("#") && !String.IsNullOrWhiteSpace(line))
                    outputList.Add(line);                        
            }        

        return outputList;
    }

    public async Task SaveDataAsync(string fileToSave, IList<string> fileList)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("#EXTM3U");
        builder.AppendLine();

        foreach (var filePath in fileList)
            builder.AppendLine(filePath);        
        
await File.WriteAllTextAsync( fileToSave, builder.ToString());
    }

}
