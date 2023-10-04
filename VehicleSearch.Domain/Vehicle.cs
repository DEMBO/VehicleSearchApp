namespace VehicleSearch.Domain;

public class Vehicle
{
    public Vehicle(string licensePlaceNumber, string brand, string type, string firstColor, string model, DateTime firstRegistrationDate)
    {
        LicensePlaceNumber = licensePlaceNumber;
        Brand = brand;
        Type = type;
        FirstColor = firstColor;
        Model = model;
        FirstRegistrationDate = firstRegistrationDate;
    }
    
    public string LicensePlaceNumber { get; }
    
    public string Brand { get; }
    
    public string Type { get; set; }
    
    public string FirstColor { get; set; }

    public string Model { get; }

    public DateTime FirstRegistrationDate { get; }
}