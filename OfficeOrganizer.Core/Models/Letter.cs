using CommunityToolkit.Mvvm.ComponentModel;

namespace OfficeOrganizer.Core.Models;
public partial class Letter : ObservableObject
{
    [ObservableProperty]
    public bool header;

    [ObservableProperty]
    public string company;

    [ObservableProperty]
    public string firstName;

    [ObservableProperty]
    public string lastName;

    [ObservableProperty]
    public string street;

    [ObservableProperty]
    public string city;

    [ObservableProperty]
    public bool signing;

    [ObservableProperty]
    public DateTimeOffset setDate;

    [ObservableProperty]
    public bool customeDate;

    [ObservableProperty]
    public string content;
}
