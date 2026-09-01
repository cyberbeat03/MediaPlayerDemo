namespace WinMix.Services;

public interface IFileOpenService
{
    IEnumerable<string> PickMediaFiles();
}