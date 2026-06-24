namespace WinMix.Services;

public interface IStorageService
{
    Task<IEnumerable<MediaItem>> LoadPlaylistAsync();
    Task SavePlaylistAsync(IEnumerable<MediaItem> fileList);
}