using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Windows.AppLifecycle;
using NLog;
using OfficeOrganizer.Core.Contracts.Services;
using OfficeOrganizer.Core.Models;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace OfficeOrganizer.ViewModels;

public partial class WriterViewModel : ObservableObject
{
    private readonly ILetterService _letterService;
    readonly Logger logger = LogManager.GetCurrentClassLogger();

    public WriterViewModel(ILetterService letterService)
    {
        Letter = new();
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;
        if (kind == ExtendedActivationKind.File)
        {
            if (args.Data is IFileActivatedEventArgs fileArgs)
            {
                IStorageItem file = fileArgs.Files.FirstOrDefault();
                Letter = LoadLetter(file!.Path);
                OnPropertyChanged(nameof(Letter));
            }
        }
        _letterService = letterService;
    }

    [ObservableProperty]
    Letter? letter;

    [ObservableProperty]
    StorageFile? storageFile;

    [ObservableProperty]
    Uri webUri;

    [RelayCommand]
    async Task SaveAs()
    {
        StorageFile = null;
        await Save();
    }

    [RelayCommand]
    async Task Save()
    {
        logger.Info("Try to save file");
        // TODO Create Methode in Letter Service
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
        {
            logger.Info("Cancel file selection");
            return;
        }

        if (StorageFile.FileType == ".md")
        {
            try
            {
                logger.Info("Try to save *.md");
                File.WriteAllText(StorageFile.Path, Letter!.Content);
            }
            catch (Exception ex)
            {
                logger.Info("Could not save file", ex);
            }
        }
        else if (StorageFile.FileType == ".xml" || StorageFile.FileType == ".ool")
        {
            logger.Info("Try to save *.xml or *.ool");
            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            FileStream file = File.Create(StorageFile.Path);
            writer.Serialize(file, Letter);
            file.Close();
        }
    }

    [RelayCommand]
    async Task Load()
    {
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(".ool");
        FilePicker.FileTypeFilter.Add(".md");
        FilePicker.FileTypeFilter.Add(".xml");

        StorageFile = await FilePicker.PickSingleFileAsync();

        Letter = LoadLetter(StorageFile.Path);

        OnPropertyChanged(nameof(Letter));
    }

    // TODO Put this to Letter Service
    Letter LoadLetter(string path)
    {
        Letter Letter = new();

        if (String.IsNullOrEmpty(path))
            return Letter;

        if (path.EndsWith(".md"))
        {
            Letter!.Content = File.ReadAllText(path);
            Letter!.Path = storageFile.Path;
            return Letter;
        }
        if (path.EndsWith(".xml") || path.EndsWith(".ool"))
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
            StreamReader xmlFile = new StreamReader(path);
            if (Letter != null)
            {
                Letter = reader.Deserialize(xmlFile) as Letter;
                Letter!.Path = path;
                return Letter;
            }
            xmlFile.Close();
        }
        return Letter!;
    }

    [RelayCommand]
    void Delete()
    {
        // TODO Create Methode in Letter Service
        StorageFile = null;
        letter = new();
        OnPropertyChanged(nameof(Letter));
    }

    [RelayCommand]
    async Task Create()
    {
        // TODO Rename into Generate
        // TODO Create Methode in Letter Service
        if (StorageFile == null)
            await Save();
        _letterService.CreatePdf(Letter);
    }

    [RelayCommand]
    public void UpdateWebView()
    {
        var html = Markdig.Markdown.ToHtml(Letter.Content);
        Console.WriteLine("");
    }

}
