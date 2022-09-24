using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.Storage;
using Windows.Storage.Pickers;

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
    async void Load()
    {
        string fileExtension = ".txt";
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(fileExtension);

        StorageFile file = await FilePicker.PickSingleFileAsync();
        Console.WriteLine($"{file}");
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{LastName} {FirstName}");
    }

}
