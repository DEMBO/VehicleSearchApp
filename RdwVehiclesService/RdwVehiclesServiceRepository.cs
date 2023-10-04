using VehicleSearch.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RdwVehiclesService.Configuration;
using RdwVehiclesService.Resources;
using RdwVehiclesService.Model;
using SODA;
using System.Reflection;
using Newtonsoft.Json;

namespace RdwVehiclesService;

public class RdwVehiclesServiceRepository : IVehicleRepository
{

    private readonly ILogger<RdwVehiclesServiceRepository> _logger;
    private readonly string _host;
    private readonly string _resourceId;
    private readonly int _maxVehiclesPerRequest;
    private readonly Resource<VehicleResponseModel> _resource;

    public RdwVehiclesServiceRepository(ILogger<RdwVehiclesServiceRepository> logger, IOptions<RdwVehicleServiceConfiguration> configuration)
    {
        _logger = logger;
        _host = configuration.Value.Host;
        _resourceId = configuration.Value.ResourceId;
        _maxVehiclesPerRequest = configuration.Value.MaxVehiclesPerRequest;

        var sodaClient = new SodaClient(_host, configuration.Value.ApiToken);
        _resource = sodaClient.GetResource<VehicleResponseModel>(_resourceId);
    }

    public IEnumerable<Vehicle> GetVehicles(string? licensePlateNumber, string? brand, string? type)
    {
        IEnumerable<VehicleResponseModel>? vehicles = null;

        var query = new SoqlQuery()
                .Select(typeof(VehicleResponseModel)
                    .GetProperties()
                    .Select(x => x.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName)
                    .ToArray())
                .Where(BuildWhereClause(licensePlateNumber, brand, type))
                .Limit(_maxVehiclesPerRequest);

        vehicles = _resource.Query<VehicleResponseModel>(query);

        if(vehicles == null)
        {
            var ex = new HttpRequestException(ErrorMessages.RdwInvalidResponse);
            _logger.LogError(ex, ex.Message);
            throw ex;
        }
        
        return vehicles.Select(v => v.ToVehicle());
    }

    private static string BuildWhereClause(string? licensePlateNumber, string? brand, string? type)
    {
        var whereParams = new List<string>();
        if(!string.IsNullOrEmpty(licensePlateNumber))
        {
            whereParams.Add($"{RdwVehicleJsonProperties.LicensePlaceNumber} like '%{licensePlateNumber}%'");
        }
        if(!string.IsNullOrEmpty(brand))
        {
            whereParams.Add($"{RdwVehicleJsonProperties.Brand}='{brand}'" );
        }
        if(!string.IsNullOrEmpty(type))
        {
            whereParams.Add($"{RdwVehicleJsonProperties.Type}='{type}'");
        }
        return string.Join(" AND ", whereParams);
    }
}
