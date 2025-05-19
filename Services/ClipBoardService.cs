using System.Collections.Specialized;

namespace WinMix.Services;

public class ClipBoardService
{
    public bool Copy(string mediaPath)
    {        
        try
        {            
            StringCollection dropFiles = new();
            dropFiles.Add(mediaPath);
            Clipboard.SetFileDropList(dropFiles);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not copy the file '{Path.GetFileName(mediaPath)}' to the clipboard.{Environment.NewLine}{ex.Message}");
            return false;
        }
    }

    public void CopyAll(IEnumerable<string> allFiles)
    {
        try
        {
            StringCollection dropFiles = new StringCollection();

            foreach (string file in allFiles)
            {
                dropFiles.Add(file);
            }

            Clipboard.SetFileDropList(dropFiles);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not copy media to the clipboard.{Environment.NewLine}{ex.Message}");
        }
    }

    public IReadOnlyList<string>? Paste()
    {
        try
        {
            if (Clipboard.ContainsFileDropList())
            {
                StringCollection fileList = Clipboard.GetFileDropList();

                return fileList.Cast<string>().ToList();
            }
            else
                {
                MessageBox.Show("The clipboard does not contain any supported media files.");
                return null;
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Error accessing the clipboard.");
            return null;
        }        
    }

}
