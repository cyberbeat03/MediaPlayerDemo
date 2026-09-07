using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace WinMix.Services;

public interface IPlaybackService : IDisposable
{
    ObservableCollection<MediaItem> Items { get; }
    int CurrentIndex { get; set; }
    string Name { get; set; }

    void AddItem(MediaItem item);
    MediaItem? GetCurrentItem();
    MediaItem? GetNextItem();
    MediaItem? GetPreviousItem();
    void MoveUp(MediaItem? mediaItem);
    void MoveDown(MediaItem? mediaItem);
    void RemoveItem(MediaItem? itemToRemove);
    IEnumerable<string> GetFilePaths();
        
    void Play();
    void Pause();
    void Stop();
    void Seek(TimeSpan position);
    void PlayNext();
    void PlayPrevious();

    TimeSpan Position { get; }
    TimeSpan Duration { get; }
    double SpeedRatio { get; set; }
    bool IsPlaying { get; }

    event EventHandler<TimeSpan>? PositionChanged;
    event EventHandler<bool>? PlayingChanged;
    event EventHandler<TimeSpan>? MediaOpened;
    event EventHandler? MediaEnded;
    event EventHandler<Exception>? MediaFailed;
    event EventHandler? CurrentItemChanged;
}
