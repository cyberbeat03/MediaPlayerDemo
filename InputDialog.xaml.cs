namespace WinMix;

public partial class InputDialog : Window
{
    public string Response { get; private set; } = string.Empty;

    public InputDialog()
    {
        InitializeComponent();
        InputText.Focus();
    }

    bool IsValidFileName =>
(!string.IsNullOrWhiteSpace(InputText.Text.Trim()) && InputText.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0);

void OnSaveButtonClick(object s, RoutedEventArgs e)
    {
        try
        {
            Response = IsValidFileName ? InputText.Text.Trim() : throw new InvalidOperationException("Please enter a valid name");

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            InputText.Focus();
        }
    }

    void OnCancelButtonClick(object s, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    void OnPreviewTextInput(object s, System.Windows.Input.TextCompositionEventArgs e)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();

        if (invalidChars.Any(c => e.Text.Contains(c)))        
            e.Handled = true;        
    }
    
}
