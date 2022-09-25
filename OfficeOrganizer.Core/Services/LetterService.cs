using OfficeOrganizer.Core.Contracts.Services;
using OfficeOrganizer.Core.Models;

namespace OfficeOrganizer.Core.Services;
public class LetterService : ILetterService
{
    public void CreatePdf(Letter letter)
    {
        string supportDir = @"c:\temp\OO";
        string htmlPath = @$"{supportDir}\temp.html";

        if (!Directory.Exists(supportDir)) { Directory.CreateDirectory(supportDir); }
        if (!Directory.Exists(@$"{supportDir}\supportFiles")) { Directory.CreateDirectory(@$"{supportDir}\supportFiles"); }

        string pdfPath = Path.ChangeExtension(letter.Path, ".pdf");

        Console.WriteLine($"{letter}");

        // TODO continue here
    }
}
