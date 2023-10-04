using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleSearch.Domain;

namespace RdwVehiclesService.Configuration;

public static class RdwVehicleServiceRegistration
{
    public const string RdwVehicleServiceConfigurationSection = "RdwVehicleService";

    public static void ConfigureRdwVehicleService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<RdwVehicleServiceConfiguration>()
            .Bind(configuration.GetSection(RdwVehicleServiceConfigurationSection))
            .ValidateDataAnnotations();
        services.AddScoped<IVehicleRepository, RdwVehiclesServiceRepository>();
    }
}
