namespace WinMix;

public partial class ListManagerDialog : Window
{    
    public ListManagerDialog(ListManagerViewModel viewModel)
    {                
        InitializeComponent();
        DataContext = viewModel;
        MediaListView.Focus();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {        
DialogResult     = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
