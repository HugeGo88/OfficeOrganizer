using Microsoft.UI.Xaml.Controls;

using OfficeOrganizer.ViewModels;

namespace OfficeOrganizer.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();
    }
}
