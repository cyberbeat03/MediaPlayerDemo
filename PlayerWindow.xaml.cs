namespace WinMix;

public partial class PlayerWindow : Window
{    
    public PlayerWindow(PlayerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        StatusText.Focus();
    }

}
