using System.Reflection;

namespace WinMix
{
    public partial class AboutWindow : Window
    {
        public string AppName { get; set; } = "WinMix Desktop";
        public string AppVersion { get; set; }
        public string BuildDate { get; set; }

        public AboutWindow()
        {
            InitializeComponent();
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
            BuildDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToLongDateString();

            DataContext = this;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
