using CommunityToolkit.Mvvm.ComponentModel;
using NLog;

namespace OfficeOrganizer.ViewModels;

public class MainViewModel : ObservableRecipient
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();

    public MainViewModel()
    {
        logger.Info("App started");
    }
}
