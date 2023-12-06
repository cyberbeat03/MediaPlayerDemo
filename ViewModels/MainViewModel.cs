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

public class MainViewModel : INotifyPropertyChanged
{
    private MainPlayer _player = new();

public MainPlayer Player
        {
        get => _player;
            set
                {
            _player = value;
            OnPropertyChanged();
            }
        }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propName = null) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

}
