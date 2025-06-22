namespace WinMix;

public partial class ListManagerWindow : Window
{
    public ListManagerWindow(ListManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }    

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }

}


