namespace WinMix.Models;

public class MediaItem(FileInfo mediaFileInfo)
{
    public string DisplayName { get; set; } = mediaFileInfo.Name;
    public string FullPath { get; set; } = mediaFileInfo.FullName;
    public Uri UriPath => new Uri(FullPath);   
    public DateTime LastAccessed { get; } = mediaFileInfo.LastAccessTime;
}
