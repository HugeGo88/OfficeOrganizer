using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OfficeOrganizer.Core.Models;
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
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(".md");
        FilePicker.FileTypeFilter.Add(".xml");

        StorageFile file = await FilePicker.PickSingleFileAsync();

        if (file.FileType == ".md")
        {
            Content = File.ReadAllText(file.Path);
        }
        if (file.FileType == ".xml")
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(file.Path);
            Letter letter = (Letter)reader.Deserialize(xmlFile);
            if (letter == null)
            {
                return;
            }
            FirstName = letter.FirstName;
            LastName = letter.LastName;
            Company = letter.Company;
            City = letter.City;
            Street = letter.Street;
            Content = letter.Content;
            SetDate = letter.SetDate;
            Header = letter.Header;
            CustomeDate = letter.CustomeDate;
            Signing = letter.Signing;
        }
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{LastName} {FirstName}");
    }

}
