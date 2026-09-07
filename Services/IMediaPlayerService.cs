using System;

namespace WinMix.Services;

public interface IMediaPlayerService : IDisposable
{
    void Open(Uri source);
    void Play();
    void Pause();
    void Stop();
    void Seek(TimeSpan position);

    TimeSpan Position { get; }
    TimeSpan Duration { get; }
    double SpeedRatio { get; set; }
    bool IsPlaying { get; }

    event EventHandler<TimeSpan>? PositionChanged;
    event EventHandler<bool>? PlayingChanged;
    event EventHandler<TimeSpan>? MediaOpened;
    event EventHandler? MediaEnded;
    event EventHandler<Exception>? MediaFailed;
}
