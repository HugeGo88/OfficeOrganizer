using Microsoft.UI.Xaml.Controls;
using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class LogPage : Page
{
    public LogViewModel ViewModel
    {
        get;
    }

    public LogPage()
    {
        ViewModel = App.GetService<LogViewModel>();
        InitializeComponent();
    }
}
