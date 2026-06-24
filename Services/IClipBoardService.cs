namespace WinMix.Services;

public interface IClipBoardService
{
    bool Copy(string mediaItem);
    void CopyAll(IEnumerable<string> allFiles);
    IEnumerable<string> Paste();
}