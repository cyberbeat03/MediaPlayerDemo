using System.Windows.Input;

namespace WinMix;

public partial class InputTextDialog : Window
{
    public string Response { get; set; } = string.Empty;

    public InputTextDialog()
    {
        InitializeComponent();
        InputText.Focus();
    }

    bool IsValidFileName =>
(!string.IsNullOrWhiteSpace(InputText.Text.Trim()) && InputText.Text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);

void OnSaveButtonClick(object s, RoutedEventArgs e)
    {
        try
        {
            Response = IsValidFileName ? Path.GetFileNameWithoutExtension(InputText.Text.Trim()) : throw new InvalidOperationException("Please enter a valid name. Do not include a file extension.");
            
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

    void OnPreviewTextInput(object s, TextCompositionEventArgs e)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();

        if (invalidChars.Any(c => e.Text.Contains(c)))        
            e.Handled = true;        
    }
    
}
