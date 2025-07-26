namespace WinMix;

public partial class ListManager : Window
{
    public ListManager(ListManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        MediaItemsList.Focus();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}