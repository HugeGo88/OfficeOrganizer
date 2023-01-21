using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OfficeOrganizer.ViewModels;

public partial class LogViewModel : ObservableObject
{
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
        if (!File.Exists(logPath)) { return; }
        try
        {
            using (var f = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var s = new StreamReader(f))
            {
                logContent = s.ReadToEnd();
            }
            OnPropertyChanged(nameof(LogContent));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

