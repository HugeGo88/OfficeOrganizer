using System.Diagnostics;
using Markdig;
using Markdig.Renderers;
using Microsoft.Win32;
using NLog;
using OfficeOrganizer.Core.Contracts.Services;
using OfficeOrganizer.Core.Models;

namespace OfficeOrganizer.Core.Services;
public class LetterService : ILetterService
{
    readonly Logger logger = LogManager.GetCurrentClassLogger();

    public void CreatePdf(Letter letter)
    {
        logger.Info("Try to generate PDF {path}", letter.Path);

        var supportDir = @"c:\temp\OO";

        if (!Directory.Exists(supportDir)) { Directory.CreateDirectory(supportDir); }
        if (!Directory.Exists(@$"{supportDir}\supportFiles")) { Directory.CreateDirectory(@$"{supportDir}\supportFiles"); }

        var pdfPath = Path.ChangeExtension(letter.Path, ".pdf");

        logger.Trace($"{letter}");
        try
        {
            var letterContent = MarkDownToHtml(letter.Content);
            RenderPdf(letterContent, pdfPath);
        }
        catch (Exception ex)
        {
            logger.Error("Could not create PDF", ex);
        }

        // TODO continue here
    }

    private string MarkDownToHtml(string content)
    {
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        MarkdownPipeline pipeline = null;
        pipeline ??= new MarkdownPipelineBuilder().UseAdvancedExtensions().UsePipeTables().Build();
        pipeline.Setup(renderer);
        var markDownHtml = Markdig.Markdown.ToHtml(content, pipeline);
        var htmlTemplate = File.ReadAllText("Assets/HtmlTemplates/index.html");
        return htmlTemplate.Replace("{{CONTENT}}", markDownHtml);
    }

    private string RenderPdf(string html, string path, string htmlTemplatePath = "")
    {
        var templatePathHtml = Path.Combine(Path.GetTempPath(), "template.html");
        var folderPath = Directory.GetParent(path).ToString();
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText($"{templatePathHtml}", html);

        var pathToExe = GetPathForExe("msedge.exe");
        var userDataDir = Path.Combine(Path.GetTempPath(), "edge-headless-user-data");
        Directory.CreateDirectory(userDataDir);

        ProcessStartInfo ps = new ProcessStartInfo
        {
            FileName = pathToExe,
            Arguments = $"--headless --disable-gpu --user-data-dir=\"{userDataDir}\" --print-to-pdf-no-header --run-all-compositor-stages-before-draw --virtual-time-budget=5000 --print-to-pdf=\"{path}\" \"{templatePathHtml}\"",
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };

        using Process converter = Process.Start(ps);
        converter.WaitForExit(); // Wait for process to finish

        var exitCode = converter.ExitCode;
        var output = converter.StandardOutput.ReadToEnd();
        var error = converter.StandardError.ReadToEnd();

        if (exitCode != 0)
        {
            // Log or display error details
            logger.Error($"Process failed with exit code {exitCode}. Error: {error}");
            //TODO needs to be fixed
            //MessageBox.Show($"PDF creation failed. Exit code: {exitCode}\nError: {error}", "PDF Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return "";
        }
        else
        {
            // Success
            //TODO needs to ne fixed
            //PdfPath = SelectedItem.PdfPath;
            return path;
        }
    }

    private string GetPathForExe(string fileName)
    {
        var keyBase = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
        RegistryKey localMachine = Registry.LocalMachine;
        RegistryKey fileKey = localMachine.OpenSubKey(String.Format(@"{0}\{1}", keyBase, fileName));
        object result = null;
        if (fileKey != null)
        {
            result = fileKey.GetValue(String.Empty);
            fileKey.Close();
        }
        return (string)result;
    }

    public void Save(Letter letter)
    {
        if (letter.FileType == ".md")
        {
            try
            {
                logger.Info("Try to save *.md to {path}", letter.Path);
                File.WriteAllText(letter.Path, letter!.Content);
            }
            catch (Exception ex)
            {
                logger.Error("Could not save file", ex);
            }
        }
        else if (letter.FileType == ".xml" || letter.FileType == ".ool")
        {
            try
            {
                logger.Info("Try to save *.xml or *.ool to {path}", letter.Path);
                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Letter));
                FileStream file = File.Create(letter.Path);
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
