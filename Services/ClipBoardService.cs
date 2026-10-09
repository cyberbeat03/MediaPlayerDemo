using System.Collections.Specialized;

namespace WinMix.Services;

public class ClipBoardService : IClipBoardService
{
    public bool Copy(string filePath)
    {
        if (String.IsNullOrEmpty(filePath)) return false;

        StringCollection dropFiles = new();
        dropFiles.Add(filePath);

        try
        {
            Clipboard.SetFileDropList(dropFiles);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return false;
        }

        return true;
    }

    public void CopyAll(IEnumerable<string> allFiles)
    {
        if (allFiles == null || !allFiles.Any())
        {
            MessageBox.Show("There are no items to copy.");
            return;
        }

        StringCollection dropFiles = new();

        foreach (string file in allFiles)
            dropFiles.Add(file);
        try
        {
            Clipboard.SetFileDropList(dropFiles);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    public IEnumerable<string> Paste()
    {
        

        try
            {
            if (!Clipboard.ContainsFileDropList()) throw new InvalidOperationException("Clipboard does not contain any files.");

            StringCollection fileList = Clipboard.GetFileDropList();
                return fileList.Cast<string>().ToList();
            }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
            return Enumerable.Empty<string>();
        }        
    }

}
