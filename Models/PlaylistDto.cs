namespace WinMix.Models;

public sealed class PlaylistDto
{
    public int Version { get; set; } = 1;
    public string Name { get; set; } = string.Empty;
    public List<MediaItemDto> Items { get; set; } = new();
}
