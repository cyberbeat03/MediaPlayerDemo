namespace WinMix.Models;

public class MediaItem
{
    public string DisplayName { get; set; }
    public string FullPath { get; set; }
    public Uri UriPath { get; set; }
    public DateTime LastAccessed { get; set; }

    public static MediaItem FromFile(string filePath)
        {                               
        if (!File.Exists( filePath))
        {
            MessageBox.Show($"The file '{Path.GetFileName(filePath)}' does not exist. It may have been moved, deleted or had it's name changed.", filePath);
            return null;
        }
        
        var fileInfo = new FileInfo(filePath);

        return new MediaItem
        {
            DisplayName = fileInfo.Name,
            FullPath = fileInfo.FullName,
            UriPath = new Uri(fileInfo.FullName),
            LastAccessed = fileInfo.LastAccessTime
        };
    }   

}
