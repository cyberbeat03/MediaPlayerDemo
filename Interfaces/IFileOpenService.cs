namespace WinMix.Interfaces;

public interface IFileOpenService
{
    IEnumerable<string> PickMediaFiles();
}