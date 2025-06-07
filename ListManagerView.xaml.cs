namespace WinMix;

public partial class ListManagerView : Window
{
    public ListManagerView(ListManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        MediaListView.Focus();
    }    

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
DialogResult = false;
    }
}


