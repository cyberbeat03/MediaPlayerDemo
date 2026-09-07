using System.Windows.Media;

namespace WinMix.Services;

public class WpfMediaPlayerService : IMediaPlayerService
{
    MediaPlayer _mPlayer = new();
    DispatcherTimer _timer;
    bool _isDisposed;
    public bool IsPlaying { get; private set; }
    public TimeSpan Position => _mPlayer.Position;
    public TimeSpan Duration
    {
        get => _mPlayer.NaturalDuration.HasTimeSpan ? _mPlayer.NaturalDuration.TimeSpan : TimeSpan.Zero;
        }        

    public double SpeedRatio
    {
        get => _mPlayer.SpeedRatio;
        set => _mPlayer.SpeedRatio = value;
    }

    public WpfMediaPlayerService()
    {
        _mPlayer.MediaOpened += OnMediaOpened;
        _mPlayer.MediaEnded += OnMediaEnded;
        _mPlayer.MediaFailed += OnMediaFailed;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (s, e) => PositionChanged?.Invoke(this, Position);
    }

    void OnMediaOpened(object? s, EventArgs e) => MediaOpened?.Invoke(this, Duration);
    void OnMediaEnded(object? s, EventArgs e) => MediaEnded?.Invoke(this, EventArgs.Empty);
    void OnMediaFailed(object? s, ExceptionEventArgs e) => MediaFailed?.Invoke(this, e.ErrorException ?? new Exception("Media failed"));

    public void Open(Uri source) => _mPlayer.Open(source);

    public void Play()
    {
        _mPlayer.Play();
        _timer.Start();
        IsPlaying = true;
        PlayingChanged?.Invoke(this, IsPlaying);
    }

    public void Pause()
    {
        _mPlayer.Pause();
        _timer.Stop();
        IsPlaying = false;
        PlayingChanged?.Invoke(this, IsPlaying);
    }

    public void Stop()
    {
        _mPlayer.Stop();
        _timer.Stop();
        IsPlaying = false;
        PlayingChanged?.Invoke(this, IsPlaying);
        _mPlayer.Position = TimeSpan.Zero;
    }

    public void Seek(TimeSpan position) => _mPlayer.Position = position;

    public event EventHandler<TimeSpan>? PositionChanged;
    public event EventHandler<bool>? PlayingChanged;
    public event EventHandler<TimeSpan>? MediaOpened;
    public event EventHandler? MediaEnded;
    public event EventHandler<Exception>? MediaFailed;

    public void Dispose()
    {
        if (_isDisposed) return;
        try
        {
            _timer.Stop();
            _mPlayer.Close();
            _mPlayer.MediaOpened -= OnMediaOpened;
            _mPlayer.MediaEnded -= OnMediaEnded;
            _mPlayer.MediaFailed -= OnMediaFailed;
        }
        catch { }
        _isDisposed = true;
    }
}
