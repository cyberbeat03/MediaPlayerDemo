namespace WinMix.Models;

public class MediaItem
{
    public string DisplayName { get; set; }
    public string FullPath { get; set; }
    public Uri UriPath { get; set; }
    public DateTime LastAccessed { get; set; }

    public static MediaItem FromFile(string filePath)
        {
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        
        var fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists) throw new FileNotFoundException("The specified file does not exist.", filePath);        

        return new MediaItem
        {
            DisplayName = fileInfo.Name,
            FullPath = fileInfo.FullName,
            UriPath = new Uri(fileInfo.FullName),
            LastAccessed = fileInfo.LastAccessTime
        };
    }   

}
