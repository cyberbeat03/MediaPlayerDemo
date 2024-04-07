namespace WinMix.Models;

public class MediaItem(FileInfo mediaFileInfo)
{
    public string MediaName { get; } = mediaFileInfo.Name;
    public string MediaPath { get; } = mediaFileInfo.FullName;
    public Uri MediaUri => new Uri(mediaFileInfo.FullName);
    public DateTime CreationDate { get; } = mediaFileInfo.CreationTime;
    public string CreationString { get; } = mediaFileInfo.CreationTime.ToLongTimeString();
}
