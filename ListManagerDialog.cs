namespace WinMix;

public partial class ListManagerDialog : Window
{    

    public ListManagerDialog()
    {                
        InitializeComponent();        
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
