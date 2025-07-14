using System.Collections.Specialized;

namespace WinMix.Services;

public class ClipBoardService
{
    public bool Copy(string mediaItem)
    {        
        StringCollection dropFiles = new();
            dropFiles.Add(mediaItem);
            Clipboard.SetFileDropList(dropFiles);
            return true;        
    }

    public void CopyAll(IEnumerable<string> allFiles)
    {
        StringCollection dropFiles = new();

            foreach (string file in allFiles)            
                dropFiles.Add(file);            

            Clipboard.SetFileDropList(dropFiles);            
    }

    public IReadOnlyList<string> Paste()
    {
        if (!Clipboard.ContainsFileDropList()) throw new InvalidOperationException("The clipboard does not contain any files.");

        StringCollection fileList = Clipboard.GetFileDropList();
        return fileList.Cast<string>().ToList();
    }

}
