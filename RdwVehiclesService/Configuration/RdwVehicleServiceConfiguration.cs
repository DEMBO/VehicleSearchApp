using System.ComponentModel.DataAnnotations;
using RdwVehiclesService.Resources;

namespace RdwVehiclesService.Configuration;

public class RdwVehicleServiceConfiguration
{
    [Required(ErrorMessage = ErrorMessages.ConfigurationRequiredValidationException)]
    public required string Host { get; set; }

    [Required(ErrorMessage = ErrorMessages.ConfigurationRequiredValidationException)]
    public required string ResourceId { get; set; }

    [Required(ErrorMessage = ErrorMessages.ConfigurationRequiredValidationException)]
    public required string ApiToken { get; set; }

    [Range(1, 5000, ErrorMessage = ErrorMessages.RangeValidationException)]
    [Required(ErrorMessage = ErrorMessages.ConfigurationRequiredValidationException)]
    public int MaxVehiclesPerRequest { get; set; }
}
