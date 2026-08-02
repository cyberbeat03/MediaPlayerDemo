namespace WinMix.Services;

public interface IStorageService
{
    Task<IEnumerable<MediaItem>> LoadPlaylistAsync(string wmxFileName);
    Task SavePlaylistAsync(string wmxFileName, IEnumerable<MediaItem> fileList);
}