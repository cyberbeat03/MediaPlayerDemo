using System.IO;
using System.Windows;

namespace WinMix;

public partial class ListManagerWindow : Window
{
    public ListManagerWindow(ViewModels.ListManagerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        PlayListView.Focus();
    }

    public string? GetSelectedPlaylistPath()
    {
        if (DataContext is ViewModels.ListManagerViewModel vm && !string.IsNullOrWhiteSpace(vm.SelectedPlaylist))
        {
            return Path.Combine(vm.PlaylistFolder, vm.SelectedPlaylist);
        }
        return null;
    }

    private void OkButton_Click(object s, RoutedEventArgs e) => DialogResult = true;
    private void CancelButton_Click(object s, RoutedEventArgs e) => DialogResult = false;
}
