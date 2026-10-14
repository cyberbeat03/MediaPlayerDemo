namespace WinMix;

public partial class ListManagerWindow : Window
{
    readonly ListManagerViewModel _viewModel;

    public ListManagerWindow(ListManagerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        DataContext = _viewModel;
        PlayListView.Focus();
        
        _viewModel.RequestClose += (s, result) =>
        {
            try
            {
                DialogResult = result;
            }
            catch { }
        };
    }

}