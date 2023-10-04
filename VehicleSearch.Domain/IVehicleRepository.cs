namespace VehicleSearch.Domain;

public interface IVehicleRepository
{
    public IEnumerable<Vehicle> GetVehicles(string? licensePlateNumber, string? brand, string? type);
}
