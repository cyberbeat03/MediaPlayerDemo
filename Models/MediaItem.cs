namespace WinMix.Models;

public class MediaItem
{
    public required string DisplayName { get; init; } = string.Empty;
    public required string FullPath { get; init; } = string.Empty;
    public required Uri UriPath { get; init; }
    public DateTime LastAccessed { get; init; }

    public static MediaItem FromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("File not found", filePath);

        var fileInfo = new FileInfo(filePath);
        return new MediaItem
        {
            DisplayName = fileInfo.Name,
            FullPath = fileInfo.FullName,
            UriPath = new Uri(fileInfo.FullName, UriKind.Absolute),
            LastAccessed = fileInfo.LastAccessTime
        };
    }

    public override bool Equals(object? obj)
    {
        return obj is MediaItem item &&
               EqualityComparer<Uri>.Default.Equals(UriPath, item.UriPath);
    }
}
