using OfficeOrganizer.Core.Models;

namespace OfficeOrganizer.Core.Contracts.Services;
public interface ILetterService
{
    void CreatePdf(Letter letter);
}
