using System.Reflection;

namespace WinMix;

public partial class AboutView : Page
{
    public string AppName { get; set; } = "WinMix Desktop";
    public string AppVersion { get; set; }
    public string BuildDate { get; set; }

    public AboutView()
    {
        InitializeComponent();
        AppName = "WinMix Desktop Player";
        AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        BuildDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToLongDateString();

        DataContext = this;
    }    
    
}
