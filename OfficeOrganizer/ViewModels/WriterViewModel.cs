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
        Letter = new();
    }

    [ObservableProperty]
    Letter letter;

    [ICommand]
    void Save()
    {
        Console.WriteLine($"{Letter.LastName} {Letter.FirstName}");
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
            Letter.Content = File.ReadAllText(file.Path);
        }
        if (file.FileType == ".xml")
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(file.Path);
            letter = (Letter)reader.Deserialize(xmlFile);
            OnPropertyChanged(nameof(Letter));
        }
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{Letter.LastName} {Letter.FirstName}");
    }

}
