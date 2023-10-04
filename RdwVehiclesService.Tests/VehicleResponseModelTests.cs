using AutoFixture.Xunit2;
using VehicleSearch.Domain;
using RdwVehiclesService.Model;

namespace RdwVehiclesService.Tests;

public class VehicleResponseModelTests
{
    [Theory, AutoData]
    public void VehicleResponseModel_ShouldConvertToDomainVehicleCorrectly(VehicleResponseModel model)
    {
        var vehicle = new Vehicle(model.LicensePlaceNumber, model.Brand, model.Type, model.FirstColor, model.Model, model.FirstRegistrationDate);
        Assert.Equivalent(model.ToVehicle(), vehicle);
    }
}