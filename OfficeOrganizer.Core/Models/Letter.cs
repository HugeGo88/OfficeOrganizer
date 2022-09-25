using CommunityToolkit.Mvvm.ComponentModel;

namespace OfficeOrganizer.Core.Models;
/// <summary>
/// Letter class that contains all information about letter content and layout
/// </summary>
public partial class Letter : ObservableObject
{
    public Letter()
    {
        // start new Letters with current date
        SetDate = DateTime.Now;
    }
    /// <summary>
    /// sets if there is a letter header or not
    /// </summary>
    [ObservableProperty]
    public bool header;

    /// <summary>
    /// contrains the company name of the letter header
    /// </summary>
    [ObservableProperty]
    public string company;

    /// <summary>
    /// sets the first name of the letter
    /// </summary>
    [ObservableProperty]
    public string firstName;

    /// <summary>
    /// sets the last name of the letter
    /// </summary>
    [ObservableProperty]
    public string lastName;

    /// <summary>
    /// sets the street of the letter
    /// </summary>
    [ObservableProperty]
    public string street;

    /// <summary>
    /// sets the city of the letter
    /// </summary>
    [ObservableProperty]
    public string city;

    /// <summary>
    /// sets if there are signatures at the end of the letter or not
    /// </summary>
    [ObservableProperty]
    public bool signing;

    /// <summary>
    /// actual date of the letter
    /// </summary>
    [ObservableProperty]
    public DateTimeOffset setDate;

    /// <summary>
    /// set a custom date for the letter or not
    /// </summary>
    [ObservableProperty]
    public bool customeDate;

    /// <summary>
    /// content of the letter
    /// </summary>
    [ObservableProperty]
    public string content;

    /// <summary>
    /// path of the letter
    /// </summary>
    [ObservableProperty]
    public string path;
}
