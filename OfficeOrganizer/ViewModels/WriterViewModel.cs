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
    Letter? letter;

    [ObservableProperty]
    StorageFile? storageFile;

    [ICommand]
    void Save()
    {
        if (StorageFile is null)
        {
            Console.WriteLine($"{Letter!.LastName} {Letter.FirstName}");
        }
        else
        {
            if (StorageFile.FileType == ".md")
            {
                File.WriteAllText(StorageFile.Path, Letter!.Content);
            }
            else if (StorageFile.FileType == ".xml")
            {
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
                System.IO.FileStream file = System.IO.File.Create(StorageFile.Path);
                writer.Serialize(file, Letter);
                file.Close();
            }
        }
    }

    [ICommand]
    async void Load()
    {
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(".md");
        FilePicker.FileTypeFilter.Add(".xml");

        StorageFile = await FilePicker.PickSingleFileAsync();

        if (StorageFile.FileType == ".md")
        {
            Letter!.Content = File.ReadAllText(StorageFile.Path);
        }
        if (StorageFile.FileType == ".xml")
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(StorageFile.Path);
            letter = (Letter)reader.Deserialize(xmlFile);
            OnPropertyChanged(nameof(Letter));
            xmlFile.Close();
        }
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{Letter!.LastName} {Letter.FirstName}");
    }

}
