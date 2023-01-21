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
        _letterService = letterService;
        AppActivationArguments args = AppInstance.GetCurrent().GetActivatedEventArgs();
        ExtendedActivationKind kind = args.Kind;
        if (kind == ExtendedActivationKind.File)
        {
            if (args.Data is IFileActivatedEventArgs fileArgs)
            {
                IStorageItem file = fileArgs.Files.FirstOrDefault();
                Letter = _letterService.Load(file!.Path);
                OnPropertyChanged(nameof(Letter));
            }
        }
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
        if (StorageFile is null)
        {
            logger.Info("Open save as dialog");
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

        letter.fileType = storageFile!.FileType;
        letter.path = storageFile.Path;
        _letterService.Save(letter);
    }

    [RelayCommand]
    async Task Load()
    {
        logger.Info("Try to load file");
        var FilePicker = App.MainWindow.CreateOpenFilePicker();
        FilePicker.ViewMode = PickerViewMode.Thumbnail;
        FilePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        FilePicker.FileTypeFilter.Add(".ool");
        FilePicker.FileTypeFilter.Add(".md");
        FilePicker.FileTypeFilter.Add(".xml");

        StorageFile = await FilePicker.PickSingleFileAsync();
        if (StorageFile == null)
        {
            return;
        }

        Letter = _letterService.Load(StorageFile.Path);

        OnPropertyChanged(nameof(Letter));
    }

    [RelayCommand]
    void Delete()
    {
        logger.Info("Delete file content");
        StorageFile = null;
        letter = new();
        OnPropertyChanged(nameof(Letter));
    }

    [RelayCommand]
    async Task Generate()
    {
        logger.Info("GeneratePdf");
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
