namespace WinMix.Services;

public class PlaybackService : IPlaybackService, IDisposable
{
    readonly IMediaPlayerService _mediaPlayer;
    public TimeSpan Position => _mediaPlayer.Position;
    public TimeSpan Duration => _mediaPlayer.Duration;
    public bool IsPlaying => _mediaPlayer.IsPlaying;
    public double SpeedRatio
    {
        get => _mediaPlayer.SpeedRatio;
        set => _mediaPlayer.SpeedRatio = value;
    }

    public ObservableCollection<MediaItem> Items { get; } = new();
    public int CurrentIndex { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
    bool _isDisposed;

    public PlaybackService(IMediaPlayerService mediaPlayer)
    {
        _mediaPlayer = mediaPlayer ?? throw new ArgumentNullException(nameof(mediaPlayer));
        _mediaPlayer.PositionChanged += (s, pos) => PositionChanged?.Invoke(this, pos);
        _mediaPlayer.MediaOpened += (s, d) => MediaOpened?.Invoke(this, d);
        _mediaPlayer.MediaEnded += (s, e) => MediaEnded?.Invoke(this, EventArgs.Empty);
        _mediaPlayer.MediaFailed += (s, ex) => MediaFailed?.Invoke(this, ex);
        _mediaPlayer.PlayingChanged += OnMediaPlayingChanged;
    }

    private bool IsIndexValid(int index) =>
        index >= 0 && index < Items.Count;

    public MediaItem? GetCurrentItem() =>
        IsIndexValid(CurrentIndex) ? Items[CurrentIndex] : null;

    public MediaItem? GetPreviousItem()
    {
        if (IsIndexValid(CurrentIndex - 1))
        {
            CurrentIndex--;
            CurrentItemChanged?.Invoke(this, EventArgs.Empty);
            return Items[CurrentIndex];
        }

        return null;
    }

    public MediaItem? GetNextItem()
    {
        if (IsIndexValid(CurrentIndex + 1))
        {
            CurrentIndex++;
            CurrentItemChanged?.Invoke(this, EventArgs.Empty);
            return Items[CurrentIndex];
        }

        return null;
    }

    public void MoveUp(MediaItem? mediaItem)
    {
        if (mediaItem is null || !Items.Contains(mediaItem)) return;

        int currentPosition = Items.IndexOf(mediaItem);
        if (currentPosition > 0)
            Items.Move(currentPosition, currentPosition - 1);
    }

    public void MoveDown(MediaItem? mediaItem)
    {
        if (mediaItem is null || !Items.Contains(mediaItem)) return;

        int currentPosition = Items.IndexOf(mediaItem);

        if (currentPosition < Items.Count - 1)
            Items.Move(currentPosition, currentPosition + 1);
    }

    public IEnumerable<string> GetFilePaths()
    {
        var paths = Items.Select(i => i.FullPath).ToList();
        foreach (var path in paths)
        {
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                yield return path;
        }
    }

    public void AddItem(MediaItem item)
    {
        if (item is null)
            return;

        if (!Items.Contains(item))
            Items.Add(item);

        if (CurrentIndex <= -1 && Items.Count > 0)
        {
            CurrentIndex = 0;
            CurrentItemChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveItem(MediaItem? itemToRemove)
    {
        if (itemToRemove is null)
            return;

        var removedIndex = Items.IndexOf(itemToRemove);
        if (removedIndex < 0)
            return;

        Items.RemoveAt(removedIndex);

        if (Items.Count == 0)
        {
            CurrentIndex = -1;
            return;
        }

        if (removedIndex < CurrentIndex)
        {
            CurrentIndex--;
            CurrentItemChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (CurrentIndex >= Items.Count)
        {
            CurrentIndex = Items.Count - 1;
            CurrentItemChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Play()
    {
        var current = GetCurrentItem();
        if (current is null && Items.Count > 0)
        {
            CurrentIndex = 0;
            current = GetCurrentItem();
        }

        if (current is null) return;

        CurrentItemChanged?.Invoke(this, EventArgs.Empty);
        _mediaPlayer.Open(current.UriPath);
        _mediaPlayer.Play();
    }

    public void Pause() => _mediaPlayer.Pause();
    public void Stop() => _mediaPlayer.Stop();
    public void Seek(TimeSpan position) => _mediaPlayer.Seek(position);

    public void PlayNext()
    {
        var next = GetNextItem();
        if (next is not null)
            Play();
    }

    public void PlayPrevious()
    {
        var prev = GetPreviousItem();
        if (prev is not null)
            Play();
    }

    public event EventHandler<TimeSpan>? PositionChanged;
    public event EventHandler<bool>? PlayingChanged;
    public event EventHandler<TimeSpan>? MediaOpened;
    public event EventHandler? MediaEnded;
    public event EventHandler<Exception>? MediaFailed;
    public event EventHandler? CurrentItemChanged;

    private void OnMediaPlayingChanged(object? s, bool playing) => PlayingChanged?.Invoke(this, playing);

    public void Dispose()
    {
        if (_isDisposed) return;
        try
        {
            _mediaPlayer.PositionChanged -= (s, pos) => PositionChanged?.Invoke(this, pos);
            _mediaPlayer.PlayingChanged -= OnMediaPlayingChanged;
        }
        catch { }

        try
        {
            _mediaPlayer.Dispose();
        }
        catch { }
        _isDisposed = true;
    }
}
