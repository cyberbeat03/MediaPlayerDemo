namespace WinMix.Services;

public class ListDataService
{    
    readonly string _playlistPath;

    public ListDataService()
    {
                 string _musicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        _playlistPath = Path.Combine(_musicPath, "playlists");
    }

    public async Task<IList<string>> LoadDataAsync(string fileToLoad)
    {
        List<string> outputList = new();        

        if (File.Exists(Path.Combine(_playlistPath, fileToLoad)))
        {
            using FileStream FS = File.OpenRead(Path.Combine(_playlistPath, fileToLoad));
            var jsonData = await JsonSerializer.DeserializeAsync<List<string>>(FS);
            if (jsonData is not null)
                outputList = jsonData;
        }        

        return outputList;                    
        }

    public async Task SaveDataAsync(string fileToSave, IList<string> fileList)
    {        
            using FileStream FS = File.Create(Path.Combine(_playlistPath, fileToSave));
            await JsonSerializer.SerializeAsync(FS, fileList);
        }    

}
