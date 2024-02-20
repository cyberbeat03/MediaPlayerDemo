namespace MediaPlayerDemo.Models;

public class MediaItem(FileInfo mediaFileInfo)
{
    public string MediaName { get; } = mediaFileInfo.Name;
    public string MediaPath { get; } = mediaFileInfo.FullName;
    public Uri MediaUri => new Uri(mediaFileInfo.FullName);
    public string MediaCreationDate { get; } = mediaFileInfo.CreationTime.ToLongDateString();
}
