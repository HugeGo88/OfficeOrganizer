using OfficeOrganizer.Core.Models;

namespace OfficeOrganizer.Core.Contracts.Services;
public interface ILetterService
{
    string CreatePdf(Letter letter);
    void Save(Letter letter);
    Letter Load(string path);
}
