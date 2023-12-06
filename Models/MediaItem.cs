using System;
using System.IO;

namespace MediaPlayerDemo.Models;

public class MediaItem
{
    public MediaItem(string fullPath)
    {
        MediaName = Path.GetFileName(fullPath);
        MediaPath = fullPath;
    }

    public string MediaName { get; set; }
    public string MediaPath { get; set; }

    public Uri MediaUri
    {
        get => new Uri(MediaPath);
    }

}
