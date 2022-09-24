namespace OfficeOrganizer.Core.Models;
public class Letter
{
    public bool Header
    {
        get; set;
    }

    public string Company
    {
        get; set;
    }

    public string FirstName
    {
        get; set;
    }

    public string LastName
    {
        get; set;
    }

    public string Street
    {
        get; set;
    }

    public string City
    {
        get; set;
    }

    public bool Signing
    {
        get; set;
    }

    public DateTimeOffset SetDate
    {
        get; set;
    }

    public bool CustomeDate
    {
        get; set;
    }

    public string Content
    {
        get; set;
    }
}
