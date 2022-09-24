using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace OfficeOrganizer.ViewModels;

public partial class WriterViewModel : ObservableObject
{
    public WriterViewModel()
    {
    }
    [ObservableProperty]
    string? firstName;

    [ObservableProperty]
    string? lastName;

    [ObservableProperty]
    string? city;

    [ObservableProperty]
    string? street;

    [ObservableProperty]
    string? company;

    [ObservableProperty]
    string? content;

    [ObservableProperty]
    DateTimeOffset setDate;

    [ObservableProperty]
    bool header;

    [ObservableProperty]
    bool customeDate;

    [ObservableProperty]
    bool signing;

    [ICommand]
    void Save()
    {
        Console.WriteLine($"{LastName} {FirstName}");
    }

    [ICommand]
    void Load()
    {
        Console.WriteLine($"{LastName} {FirstName}");
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{LastName} {FirstName}");
    }

}
