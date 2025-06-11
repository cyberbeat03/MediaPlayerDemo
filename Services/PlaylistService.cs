namespace WinMix.Services;

public class PlaylistService
{
    private readonly string _playlistFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
        "playlists");

    private bool EnsurePlaylistFolder()
    {
        try
        {
            if (!Directory.Exists(_playlistFolder))
                Directory.CreateDirectory(_playlistFolder);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not create or access playlist folder: {ex.Message}");
            return false;
        }
    }

    public async Task<IReadOnlyList<string>> LoadAsync(string fileName)
    {
        var outputList = new List<string>();
        if (!EnsurePlaylistFolder()) return outputList;

        var fullPath = Path.Combine(_playlistFolder, fileName);
        try
        {
            if (File.Exists(fullPath))
            {
                var lines = await File.ReadAllLinesAsync(fullPath);
                foreach (var line in lines)
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                        outputList.Add(line.Trim());
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not load {fileName}: {ex.Message}");
        }
        return outputList;
    }

    public async Task<bool> SaveAsync(string fileName, IEnumerable<string> fileList)
    {
        if (!EnsurePlaylistFolder()) return false;
        var fullPath = Path.Combine(_playlistFolder, fileName);
        try
        {
            var builder = new StringBuilder();
            builder.AppendLine("#EXTM3U");
            builder.AppendLine();
            foreach (var filePath in fileList)
                builder.AppendLine(filePath);
            await File.WriteAllTextAsync(fullPath, builder.ToString());
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not save playlist: {ex.Message}");
            return false;
        }
    }
}
