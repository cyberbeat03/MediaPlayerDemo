using MediaPlayerDemo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MediaPlayerDemo.ViewModels;

public class MainPlayer : INotifyPropertyChanged
{
    private string _displayStatus = string.Empty;
    private string _totalDuration = "00:00";
    private string _elapsedTime = "00:00";
    private DispatcherTimer _timer = new();
    private MediaElement _mPlayer = new();    
    private PlaybackList _mediaList = new();

    public MediaElement MPlayer
    {
        get => _mPlayer;
        set
        {
            _mPlayer = value;
            OnPropertyChanged();
        }
    }

    public PlaybackList MediaList
    {
        get => _mediaList;
        set
        {
            _mediaList = value;
            OnPropertyChanged();
        }
    }

    public string ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            _elapsedTime = value;
            OnPropertyChanged();
        }
    }

    public string TotalDuration
    {
        get => _totalDuration;
        set
        {
            _totalDuration = value;
            OnPropertyChanged();
        }
    }

    public string DisplayStatus
    {
        get => _displayStatus;
        set
        {
            if (_displayStatus != value)
            {
                _displayStatus = value;
                OnPropertyChanged();
            }
        }
    }

    public bool CanRepeat { get; set; }

    public MainPlayer()
    {
        MPlayer.Volume = 0.5;
        MPlayer.Balance = 0;
        MPlayer.SpeedRatio = 1;
        MPlayer.LoadedBehavior = MediaState.Manual;

        MPlayer.MediaOpened += Media_Opened;
        MPlayer.MediaEnded += Media_Ended;

        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        GetMediaDetails();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (MPlayer.NaturalDuration.HasTimeSpan)
        {
            ElapsedTime = MPlayer.Position.ToString(@"mm\:ss");
        }
    }

    public void ChangeSpeed(double newValue)
    {
        MPlayer.SpeedRatio = newValue;
    }
    
        public void AddMediaFiles()
    {
        FileOperations fileOperations = new();

        IList<string> pickedFiles = fileOperations.PickMediaFiles();

        if (pickedFiles.Count != 0)
        {
            MediaList.AddFilesToList(pickedFiles);
            PlayItem(MediaList.CurrentItem);
        }
    }

    public void GetMediaDetails()
    {
        if (MPlayer.Source is null)
        {
            DisplayStatus = "Nothing to play";
        }
        else
        {
            DisplayStatus = $"{MPlayer.Source}";
        }
    }

    public void Play()
    {
        if (MPlayer.Source is not null)
        {
            if (_timer.IsEnabled == false)
            {
                _timer.Start();
            }

            MPlayer.Play();
        }
    }

    public void Pause()
    {
        if (MPlayer.Source is not null)
        {
            _timer.Stop();
            MPlayer.Pause();        
    }
    }

    public void Rewind()
    {
        MPlayer.Position -= TimeSpan.FromSeconds(10);
    }

    public void FastForward()
    {
        MPlayer.Position += TimeSpan.FromSeconds(10);
    }

    public void PlayNext()
    {
        if (MPlayer.Source is not null)
        {            
            PlayItem(MediaList.GetNextItem());
        }
    }

    public void PlayPrevious()
    {
        if (MPlayer.Source is not null)
        {            
            PlayItem(MediaList.GetPreviousItem());
        }
    }

    private void PlayItem(MediaItem? currentItem)
    {
        if (currentItem != null && MPlayer.Source != currentItem.MediaUri)
        {
            MPlayer.Source = currentItem.MediaUri;
                Play();
            }
GetMediaDetails();
    }

    private void Media_Opened(object sender, RoutedEventArgs e)
    {
        TotalDuration = MPlayer.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
        _timer.Start();        
    }

    private void Media_Ended(object sender, RoutedEventArgs e)
    {
        MPlayer.Stop();
        _timer.Stop();
        ElapsedTime = "00:00";

        if (CanRepeat)
        {
                Play();            
        }
        else
        {
            PlayNext();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propName = null) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

}
