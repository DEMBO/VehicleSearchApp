using System.Net;
using Microsoft.AspNetCore.Mvc;
using VehicleSearch.Domain;

namespace VehicleSearch.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/vehicles")]
public class VehicleSearchController : ControllerBase
{
    private readonly ILogger<VehicleSearchController> _logger;
    private readonly IVehicleRepository _vehicleRepository;

    public VehicleSearchController(ILogger<VehicleSearchController> logger, IVehicleRepository vehicleRepository)
    {
        _logger = logger;
        _vehicleRepository = vehicleRepository;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Vehicle>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<Vehicle>> Get(string? licensePlateNumber, string? brand, string? type)
    {
        IEnumerable<Vehicle> vehicles;
        try
        {
            vehicles = _vehicleRepository.GetVehicles(licensePlateNumber, brand, type);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        return Ok(vehicles);
    }
}
