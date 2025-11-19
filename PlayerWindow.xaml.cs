namespace WinMix;

public partial class PlayerWindow : Window
{
    private readonly PlayerViewModel _viewModel;

    public PlayerWindow(PlayerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = viewModel;
        StatusText.Focus();

        // Ensure ViewModel is disposed when window is closed to prevent leaks
        this.Closed += PlayerWindow_Closed;
    }

    private void PlayerWindow_Closed(object? sender, EventArgs e)
    {
        try
        {
            _viewModel?.Dispose();
        }
        catch
        {
            // Swallow exceptions during dispose to avoid crashing the UI on close
        }
    }

}
