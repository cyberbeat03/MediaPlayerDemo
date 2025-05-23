namespace WinMix.Models;

public class MediaItem(FileInfo mediaFileInfo)
{
    public string DisplayName { get; set; } = mediaFileInfo.Name;
    public string FullPath { get; } = mediaFileInfo.FullName;
    public Uri UriPath { get; } = new Uri(mediaFileInfo.FullName, UriKind.Absolute);
    public DateTime LastAccessed { get; } = mediaFileInfo.LastAccessTime;
}
