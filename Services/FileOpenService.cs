using Microsoft.Win32;

namespace WinMix.Services;

public class FileOpenService
{
    private readonly string _musicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    private readonly string _supportedFileTypes = "Audio Files (*.MP3;*.M4A;*.AAC;*.FLAC;*.WMA;*.WAV)|*.mp3;*.m4a;*.aac;*.flac;*.wma;*.wav|Video Files (*.MP4;*.WMV)|*.mp4;*.wmv";

    public IEnumerable<string> PickMediaFiles()
    {        
        OpenFileDialog OFD = new()
        {
            InitialDirectory = _musicFolder,
            Filter = _supportedFileTypes,
            Title = "Select media files to add to playlist",
            RestoreDirectory = true,
            Multiselect = true,
            CheckFileExists = true,
             ValidateNames= true,
            DefaultExt = ".mp3"
        };

        if (OFD.ShowDialog() == true)
            return OFD.FileNames.ToList();

        return Enumerable.Empty<string>();
    }

    public string PickPlaylistFile()
    {
        OpenFileDialog OFD = new()
        {
            Title = "Select a playlist file to load",
            InitialDirectory = Path.Combine(_musicFolder, "Playlists"),
            Multiselect = false,
            RestoreDirectory = true,
            CheckFileExists = true,            
            DefaultExt = ".m3u8",
            Filter = "Playlist Files (*.WPL;*.M3U;*.M3U8)|*.wpl;*.m3u;*.m3u8"
        };   
        
        if (OFD.ShowDialog() == true)
        {
            return OFD.FileName ?? string.Empty;
        }

        return string.Empty;
    }

}
