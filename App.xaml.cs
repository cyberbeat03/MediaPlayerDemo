using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WinMix;

public partial class App : Application
{
    private IHost? _host;

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {                
                services.AddSingleton<IPlaybackService, PlaybackService>();
                services.AddSingleton<IFileOpenService, FileOpenService>();
                services.AddSingleton<IStorageService, StorageService>();
                services.AddSingleton<IClipBoardService, ClipBoardService>();
                services.AddSingleton<IWindowDisplayService, WindowDisplayService>();

                services.AddTransient<ListManagerWindow>();
                services.AddTransient<ListManagerViewModel>();

                services.AddTransient<PlayerWindow>();
                services.AddTransient<PlayerViewModel>();
            })
            .Build();

        await _host.StartAsync();

                var window = _host.Services.GetRequiredService<PlayerWindow>();                
        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            await _host.StopAsync(TimeSpan.FromSeconds(5));
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
   