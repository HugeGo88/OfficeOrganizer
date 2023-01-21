using OfficeOrganizer.Helpers;

namespace OfficeOrganizer;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/OfficeOrganizer.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
