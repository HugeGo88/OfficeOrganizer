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
            var readLines = File.ReadAllLines(logPath);
            logContent = string.Join("\n", readLines?.Reverse() ?? new string[0]);
            OnPropertyChanged(nameof(LogContent));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

