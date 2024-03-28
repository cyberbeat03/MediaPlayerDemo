namespace WinMix.Services;

public class PlaylistService
{
    readonly string _playlistFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
    "playlists");

    public async Task<IList<string>> LoadDataAsync(string fileToLoad)
    {
        List<string> outputList = new();

        if (File.Exists(fileToLoad))
        {
            using FileStream FS = File.OpenRead(fileToLoad);
            var jsonData = await JsonSerializer.DeserializeAsync<List<string>>(FS);

            if (jsonData is not null)
                outputList = jsonData;
        }

        return outputList;
    }

    public async Task SaveDataAsync(string fileToSave, IList<string> fileList)
    {
        using FileStream FS = File.Create(fileToSave);
        await JsonSerializer.SerializeAsync(FS, fileList);
    }    

}
