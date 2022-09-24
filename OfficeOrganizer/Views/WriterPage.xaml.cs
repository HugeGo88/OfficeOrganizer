using Microsoft.UI.Xaml.Controls;
using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Views;
public sealed partial class WriterPage : Page
{
    public WriterViewModel ViewModel
    {
        get;
    }

    public WriterPage()
    {
        ViewModel = App.GetService<WriterViewModel>();
        InitializeComponent();
    }
}
