using Microsoft.UI.Xaml.Controls;

using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}
