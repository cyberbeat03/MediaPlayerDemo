using System.Collections.Specialized;

namespace WinMix.Services;

public class ClipBoardService
{
    public bool Cut(string mediaPath)
    {
        try
        {
            StringCollection dropList = new();
            dropList.Add(mediaPath);
            Clipboard.SetFileDropList(dropList);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not copy file to the clipboard. {Environment.NewLine}{ex.Message}");
            return false;
        }
    }

    public void Copy(string mediaPath)
    {
        try
        {
            StringCollection dropFiles = new();
            dropFiles.Add(mediaPath);

            Clipboard.SetFileDropList(dropFiles);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not copy files to the clipboard.{Environment.NewLine}{ex.Message}");
        }
    }

    public void CopyAll(IReadOnlyList<string> allFiles)
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
        }
        catch (Exception)
        {
            MessageBox.Show("Could not find any supported media files on the clipboard.");
            return null;
        }
        return null;
    }

}
