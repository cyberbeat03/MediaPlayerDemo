namespace WinMix.Services;

public class ListDataService
{    
    readonly string _storagePath;

    public ListDataService()
    {
                 string _musicPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        _storagePath = Path.Combine(_musicPath, "playlists");
    }

    public async Task<IList<string>> LoadDataAsync(string fileToLoad)
    {
        List<string> outputList = new();
        string playlistPath = Path.Combine(_storagePath, fileToLoad);

        if (File.Exists(playlistPath))
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
        string playlistPath = Path.Combine(_storagePath, fileToSave);
        
            using FileStream FS = File.Create(playlistPath);
            await JsonSerializer.SerializeAsync(FS, fileList);
        }    
}
