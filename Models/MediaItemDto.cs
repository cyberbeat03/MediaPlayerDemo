namespace WinMix.Models;

public sealed class MediaItemDto
{
    public string DisplayName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;    
    public long LastAccessedFileTimeUtc { get; set; }
}
