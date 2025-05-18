using Microsoft.Win32;

namespace WinMix.Services;

public class FileOpenService
{
    private readonly string _musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    private readonly string title = "Select media files to add to playlist";
    private readonly string supportedFileTypes = "Audio Files (*.MP3;*.MP4;*.M4A;*.AAC;*.FLAC;*.ALAC;*.WMA;*.WAV)|*.mp3;*.mp4;*.m4a;*.aac;*.flac;*.alac;*.wma;*.wav|All Files (*.*)|*.*";

    public IReadOnlyList<string> PickMediaFiles()
    {
        List<string> outputList = new();

        OpenFileDialog OFD = new()
        {
            InitialDirectory = _musicFolder,
            Filter = supportedFileTypes,
            Title = title,
            RestoreDirectory = true,
            Multiselect = true,
            CheckFileExists = true,
            DefaultExt = ".mp3"
        };

        if (OFD.ShowDialog() == true)
        {
            outputList = OFD.FileNames.ToList();
        }

        return outputList;
    }

}
