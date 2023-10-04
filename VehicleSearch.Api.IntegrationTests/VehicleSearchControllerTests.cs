using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using RdwVehiclesService.Configuration;
using RdwVehiclesService.Model;
using VehicleSearch.Api.Middleware;
using VehicleSearch.Api.Resources;

namespace VehicleSearch.Api.IntegrationTests;

public class VehicleSearchControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private string _apiKey;
    private int _maxVehiclesPerRequest;

    private const string _apiUrl = "/api/vehicles";

    public VehicleSearchControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        _apiKey = config[ApiKeyMiddleware.ApiKeyName] ?? "";
        _maxVehiclesPerRequest = config.GetSection(RdwVehicleServiceRegistration.RdwVehicleServiceConfigurationSection).Get<RdwVehicleServiceConfiguration>()?.MaxVehiclesPerRequest ?? 1;
    }

    [Fact]
    public async void GetVehicles_WithoutApiKey_ShouldReturnUnauthorized()
    {
        using var client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync(_apiUrl);
        
        Assert.Equivalent(response.StatusCode, HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async void GetVehicles_CorrectData_ShouldReturnVehicle()
    {
        IEnumerable<VehicleResponseModel>? vehicles;
        var licensePlateNumber = "0001TJ";

        var query = new Dictionary<string, string?>()
        {
            ["licensePlateNumber"] = licensePlateNumber,
            ["brand"] = "FORD",
            ["type"] = "Personenauto"
        };

        var uri = QueryHelpers.AddQueryString(_apiUrl, query);

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add(GenericMessages.ApiKey, _apiKey);

        HttpResponseMessage response = await client.GetAsync(uri);
        
        response.EnsureSuccessStatusCode();

        vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<VehicleResponseModel>>();

        Assert.NotNull(vehicles);
        Assert.NotEmpty(vehicles);
        Assert.Equivalent(vehicles.Count(), 1);
        Assert.Equivalent(vehicles.Single().LicensePlaceNumber, licensePlateNumber);
    }

    [Fact]
    public async void GetVehicles_IncorrectData_ShouldReturnNoVehicles()
    {
        IEnumerable<VehicleResponseModel>? vehicles;
        var licensePlateNumber = "lasdkjfhuaweukfqeiruadsfmbxcvlkhasf";

        var query = new Dictionary<string, string?>()
        {
            ["licensePlateNumber"] = licensePlateNumber,
        };

        var uri = QueryHelpers.AddQueryString(_apiUrl, query);

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add(GenericMessages.ApiKey, _apiKey);

        HttpResponseMessage response = await client.GetAsync(uri);
        
        response.EnsureSuccessStatusCode();

        vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<VehicleResponseModel>>();

        Assert.NotNull(vehicles);
        Assert.Empty(vehicles);
    }

    [Fact]
    public async void GetVehicles_EmptyFields_ShouldReturnMaxLimitVehicles()
    {
        IEnumerable<VehicleResponseModel>? vehicles;

        using var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add(GenericMessages.ApiKey, _apiKey);

        HttpResponseMessage response = await client.GetAsync(_apiUrl);
        
        response.EnsureSuccessStatusCode();

        vehicles = await response.Content.ReadFromJsonAsync<IEnumerable<VehicleResponseModel>>();

        Assert.NotNull(vehicles);
        Assert.NotEmpty(vehicles);
        Assert.Equivalent(vehicles.Count(), _maxVehiclesPerRequest);
    }
}