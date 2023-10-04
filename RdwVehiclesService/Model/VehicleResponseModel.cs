using Newtonsoft.Json;
using VehicleSearch.Domain;

namespace RdwVehiclesService.Model;

public class VehicleResponseModel
{
    public VehicleResponseModel(string licensePlaceNumber, string brand, string type, string firstColor, string model, DateTime firstRegistrationDate)
    {
        LicensePlaceNumber = licensePlaceNumber;
        Brand = brand;
        Type = type;
        FirstColor = firstColor;
        Model = model;
        FirstRegistrationDate = firstRegistrationDate;
    }
    
    [JsonProperty(RdwVehicleJsonProperties.LicensePlaceNumber)]
    public string LicensePlaceNumber { get; }
    
    [JsonProperty(RdwVehicleJsonProperties.Brand)]
    public string Brand { get; }
    
    [JsonProperty(RdwVehicleJsonProperties.Type)]
    public string Type { get; }
    
    [JsonProperty(RdwVehicleJsonProperties.FirstColor)]
    public string FirstColor { get; }

    [JsonProperty(RdwVehicleJsonProperties.Model)]
    public string Model { get; }

    [JsonProperty(RdwVehicleJsonProperties.FirstRegistrationDate)]
    public DateTime FirstRegistrationDate { get; }

    public Vehicle ToVehicle()
    {
        return new Vehicle(LicensePlaceNumber, Brand, Type, FirstColor, Model, FirstRegistrationDate);
    }
}