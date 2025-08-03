namespace WinMix;

public partial class ListManagerWindow : Window
{
    public ListManagerWindow()
    {
        InitializeComponent();
        DataContext = new ListManagerViewModel(new PlaybackList());
        MediaItemsList.Focus();
    }

}