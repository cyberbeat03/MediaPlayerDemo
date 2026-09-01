namespace WinMix.Services;
using System.Threading.Tasks;

public interface IWindowDisplayService
{        
    void ShowAboutDialog();
    string ShowInputDialog();
    Task<string?> PickPlaylistFileAsync();
}
