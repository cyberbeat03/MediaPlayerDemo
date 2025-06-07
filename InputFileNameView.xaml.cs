namespace WinMix;

public partial class InputFileNameView : Window
{
    public string Response { get; private set; } = string.Empty;

    public InputFileNameView()
    {
        InitializeComponent();
        InputBox.Focus();
    }

    bool IsValidFileName =>
(!string.IsNullOrWhiteSpace(InputBox.Text.Trim()) && InputBox.Text.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0);

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Response = IsValidFileName ? InputBox.Text.Trim() : throw new InvalidOperationException("Please enter a valid name");

            DialogResult = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            InputBox.Focus();
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
