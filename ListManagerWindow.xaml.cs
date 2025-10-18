namespace WinMix;

public partial class ListManagerWindow : Window
{
    public ListManagerWindow(ListManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        MediaItemsList.Focus();
    }

}