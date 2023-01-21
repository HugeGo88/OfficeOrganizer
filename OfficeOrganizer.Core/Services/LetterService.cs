using NLog;
using OfficeOrganizer.Core.Contracts.Services;
using OfficeOrganizer.Core.Models;

namespace OfficeOrganizer.Core.Services;
public class LetterService : ILetterService
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();

    public void CreatePdf(Letter letter)
    {
        var supportDir = @"c:\temp\OO";
        var htmlPath = @$"{supportDir}\temp.html";

        if (!Directory.Exists(supportDir)) { Directory.CreateDirectory(supportDir); }
        if (!Directory.Exists(@$"{supportDir}\supportFiles")) { Directory.CreateDirectory(@$"{supportDir}\supportFiles"); }

        var pdfPath = Path.ChangeExtension(letter.Path, ".pdf");

        Console.WriteLine($"{letter}");

        // TODO continue here
    }

    public void Save(Letter letter)
    {
        if (letter.fileType == ".md")
        {
            try
            {
                logger.Info("Try to save *.md to {path}", letter.path);
                File.WriteAllText(letter.path, letter!.Content);
            }
            catch (Exception ex)
            {
                logger.Error("Could not save file", ex);
            }
        }
        else if (letter.fileType == ".xml" || letter.fileType == ".ool")
        {
            try
            {
                logger.Info("Try to save *.xml or *.ool to {path}", letter.path);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
                FileStream file = File.Create(letter.path);
                writer.Serialize(file, letter);
                file.Close();
            }
            catch (Exception ex)
            {
                logger.Error("Could not save file", ex);
            }
        }
    }

    public Letter Load(string path)
    {
        try
        {
            Letter Letter = new();

            if (String.IsNullOrEmpty(path))
                return Letter;

            if (path.EndsWith(".md"))
            {
                logger.Info("Try to load md file {path}", path);
                Letter!.Content = File.ReadAllText(path);
                Letter!.Path = path;
                return Letter;
            }
            if (path.EndsWith(".xml") || path.EndsWith(".ool"))
            {
                logger.Info("Try to load xml or ool file {path}", path);
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
        catch (Exception ex)
        {
            logger.Info("Try to load failed {exception}", ex.Message);
            return new Letter();
        }
    }
}
