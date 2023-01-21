using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NLog;

namespace OfficeOrganizer.ViewModels;

public partial class LogViewModel : ObservableObject
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();

    public LogViewModel()
    {
        logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"OfficeOrganizer\logData.log");
        logContent = string.Empty;
        LoadLog();
    }

    [ObservableProperty]
    public string logContent;

    [ObservableProperty]
    public string logPath;

    [RelayCommand]
    void LoadLog()
    {
        if (!File.Exists(LogPath)) { return; }
        try
        {
            using (var f = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var s = new StreamReader(f))
            {
                LogContent = s.ReadToEnd();
            }
            LogContent = string.Join("\r\n", LogContent.Split('\r', '\n').Reverse());
            OnPropertyChanged(nameof(LogContent));
        }
        catch (Exception ex)
        {
            logger.Error("Couldn't read log file {message}", ex.Message);
        }
    }
}

