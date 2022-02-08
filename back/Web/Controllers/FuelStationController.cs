using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Web.Controllers;

[ApiController]
[Route("api/fuel-stations", Name = "FuelStations")]
public class FuelStationController : ControllerBase
{
    private readonly IFuelStationService client;

    public FuelStationController(IFuelStationService client)
    {
        this.client = client;
    }

    /// <summary>
    /// Get All fuel stations around a point
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="radius">Maximal distance in meter between the fuel station and the specified point</param>
    [HttpGet]
    [ProducesResponseType(typeof(List<FuelStationData>), 200)]
    public async Task<IActionResult> GetFuelStations([Required] double latitude, [Required] double longitude, long radius = 10000)
    {
        return Ok(await client.GetFuelStations(latitude, longitude, radius));

    }
}