using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OfficeOrganizer.Core.Contracts.Services;
using OfficeOrganizer.Core.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace OfficeOrganizer.ViewModels;

public partial class WriterViewModel : ObservableObject
{
    private readonly ILetterService _letterService;

    public WriterViewModel(ILetterService letterService)
    {
        Letter = new();
        _letterService = letterService;
    }

    [ObservableProperty]
    Letter? letter;

    [ObservableProperty]
    StorageFile? storageFile;

    [ICommand]
    async Task SaveAs()
    {
        StorageFile = null;
        await Save();
    }

    [ICommand]
    async Task Save()
    {

        if (StorageFile is null)
        {
            var FilePicker = App.MainWindow.CreateSaveFilePicker();
            FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            FilePicker.SuggestedFileName = "NewLetter";
            FilePicker.DefaultFileExtension = ".ool";
            FilePicker.FileTypeChoices.Add("OOL", new List<string>() { ".ool" });
            FilePicker.FileTypeChoices.Add("XML", new List<string>() { ".xml" });
            FilePicker.FileTypeChoices.Add("Markdown", new List<string>() { ".md" });
            FilePicker.SuggestedFileName = "New Document";
            StorageFile = await FilePicker.PickSaveFileAsync();
            if (StorageFile?.Path != null)
                Letter!.Path = StorageFile.Path;
        }
        if (StorageFile is null)
            return;

        if (StorageFile.FileType == ".md")
        {
            File.WriteAllText(StorageFile.Path, Letter!.Content);
        }
        else if (StorageFile.FileType == ".xml" || StorageFile.FileType == ".ool")
        {
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            FileStream file = File.Create(StorageFile.Path);
            writer.Serialize(file, Letter);
            file.Close();
        }
    }

    [ICommand]
    async Task Load()
    {
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(".ool");
        FilePicker.FileTypeFilter.Add(".md");
        FilePicker.FileTypeFilter.Add(".xml");

        StorageFile = await FilePicker.PickSingleFileAsync();

        if (StorageFile is null)
            return;

        if (StorageFile.FileType == ".md")
        {
            Letter!.Content = File.ReadAllText(StorageFile.Path);
            Letter!.Path = StorageFile.Path;
        }
        if (StorageFile.FileType == ".xml" || StorageFile.FileType == ".ool")
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(StorageFile.Path);
            if (Letter != null)
            {
                Letter = reader.Deserialize(xmlFile) as Letter;
                Letter!.Path = StorageFile.Path;
            }
            xmlFile.Close();
        }
        OnPropertyChanged(nameof(Letter));
    }

    [ICommand]
    void Delete()
    {
        StorageFile = null;
        letter = new();
        OnPropertyChanged(nameof(Letter));
    }

    [ICommand]
    async Task Create()
    {
        if (StorageFile == null)
            await Save();
        _letterService.CreatePdf(Letter);
    }

}
