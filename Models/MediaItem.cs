namespace WinMix.Models;

public class MediaItem(FileInfo mediaFileInfo)
{
    public string DisplayName { get; set; } = mediaFileInfo.Name;
    public string FullPath { get; set; } = mediaFileInfo.FullName;
    public Uri UriPath => new Uri(FullPath);
    public DateTime CreationDate { get; } = mediaFileInfo.CreationTime;
    public string CreationString { get; } = mediaFileInfo.CreationTime.ToLongTimeString();
}
