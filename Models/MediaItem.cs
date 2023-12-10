using System;
using System.IO;

namespace MediaPlayerDemo.Models;

public class MediaItem(string fullPath)
{
    public string MediaName { get; } = Path.GetFileName(fullPath);
    public string MediaPath { get; } = fullPath;
    public Uri MediaUri => new Uri(MediaPath);
}
