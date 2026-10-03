using System.Text.Json.Serialization;

namespace WinMix.Models;

public class MediaItem
{    
    public string DisplayName { get; init; } = string.Empty;
    public string FullPath { get; init; } = string.Empty;
    public Uri UriPath { get; init; }
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

    [JsonConstructor]
    public MediaItem(string displayName, string fullPath, DateTime lastAccessed)
    {
        DisplayName = displayName ?? string.Empty;
        FullPath = fullPath ?? string.Empty;
        LastAccessed = lastAccessed;
        UriPath = new Uri(FullPath, UriKind.Absolute);
    }

    public MediaItem() { UriPath = new Uri("", UriKind.RelativeOrAbsolute); }

    public override bool Equals(object? obj)
    {
        return obj is MediaItem item &&
               EqualityComparer<Uri>.Default.Equals(UriPath, item.UriPath);
    }

    public override int GetHashCode()
    {
        return UriPath.GetHashCode();
    }
}
