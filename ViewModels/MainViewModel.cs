using MediaPlayerDemo.Models;

namespace MediaPlayerDemo.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private MainPlayer _player;

public MainPlayer Player
        {
        get => _player;
            set
                {
            _player = value;
            OnPropertyChanged();
            }
        }

    public MainViewModel()
    {
       _player = new(new PlaybackList());       
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propName = null) =>
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

}
