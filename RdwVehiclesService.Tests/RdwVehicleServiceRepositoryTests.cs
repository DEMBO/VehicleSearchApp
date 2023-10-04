using RdwVehiclesService.Model;
using AutoFixture;
using System.Reflection;

namespace RdwVehiclesService.Tests;

public class RdwVehicleServiceRepositoryTests
{   
    private static IFixture _fixture = new Fixture();

    public static IEnumerable<object?[]> BuildWhereClauseData()
    {
        var licensePlateNumber = _fixture.Create<string>();
        var brand = _fixture.Create<string>();
        var type = _fixture.Create<string>();
        yield return new object?[] { licensePlateNumber, brand, type, $"{RdwVehicleJsonProperties.LicensePlaceNumber} like '%{licensePlateNumber}%' AND {RdwVehicleJsonProperties.Brand}='{brand}' AND {RdwVehicleJsonProperties.Type}='{type}'"};
        yield return new object?[] { licensePlateNumber, null, type, $"{RdwVehicleJsonProperties.LicensePlaceNumber} like '%{licensePlateNumber}%' AND {RdwVehicleJsonProperties.Type}='{type}'"};
        yield return new object?[] { licensePlateNumber, brand, null, $"{RdwVehicleJsonProperties.LicensePlaceNumber} like '%{licensePlateNumber}%' AND {RdwVehicleJsonProperties.Brand}='{brand}'"};
        yield return new object?[] { null, brand, type, $"{RdwVehicleJsonProperties.Brand}='{brand}' AND {RdwVehicleJsonProperties.Type}='{type}'"};
        yield return new object?[] { "", brand, "", $"{RdwVehicleJsonProperties.Brand}='{brand}'"};
        yield return new object?[] { null, null, null, $""};
    }

    [Theory]
    [MemberData(nameof(BuildWhereClauseData))]
    public void BuildWhereClause_DifferentParameters_ShouldBuildClauseCorrectly(string? licensePlateNumber, string? brand, string? type, string expected)
    {
        MethodInfo method = typeof(RdwVehiclesServiceRepository).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
          .Where(x => x.Name == "BuildWhereClause" && x.IsPrivate)
          .First();

        var whereClause = (string?)method.Invoke(null, new object?[] { licensePlateNumber, brand, type });

        Assert.Equivalent(whereClause, expected);
    }
}
