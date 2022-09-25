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
    async void Save()
    {
        if (StorageFile is null)
        {
            var FilePicker = App.MainWindow.CreateSaveFilePicker();
            FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            FilePicker.FileTypeChoices.Add("XML", new List<string>() { ".xml" });
            FilePicker.FileTypeChoices.Add("Markdown", new List<string>() { ".md" });
            FilePicker.SuggestedFileName = "New Document";
            StorageFile = await FilePicker.PickSaveFileAsync();
        }
        if (StorageFile is null)
            return;

        if (StorageFile.FileType == ".md")
        {
            File.WriteAllText(StorageFile.Path, Letter!.Content);
        }
        else if (StorageFile.FileType == ".xml")
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            FileStream file = File.Create(StorageFile.Path);
            writer.Serialize(file, Letter);
            file.Close();
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

        if (StorageFile is null)
            return;

        if (StorageFile.FileType == ".md")
        {
            Letter!.Content = File.ReadAllText(StorageFile.Path);
        }
        if (StorageFile.FileType == ".xml")
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(StorageFile.Path);
            if (Letter != null)
            {
                Letter = reader.Deserialize(xmlFile) as Letter;
            }
            OnPropertyChanged(nameof(Letter));
            xmlFile.Close();
        }
    }

    [ICommand]
    void New()
    {
        StorageFile = null;
        letter = new();
        OnPropertyChanged(nameof(Letter));
    }

    [ICommand]
    void Create()
    {
        Console.WriteLine($"{Letter!.LastName} {Letter.FirstName}");
    }

}
