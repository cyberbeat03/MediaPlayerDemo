namespace WinMix.Models;

public sealed class Playlist
{
    public int Version { get; set; } = 1;
    public string Name { get; set; } = string.Empty;
    public List<MediaItem> Items { get; set; } = new();
}
