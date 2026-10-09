namespace WinMix.Interfaces;

public interface IClipBoardService
{
    bool Copy(string filePath   );
    void CopyAll(IEnumerable<string> allFiles);
    IEnumerable<string> Paste();
}