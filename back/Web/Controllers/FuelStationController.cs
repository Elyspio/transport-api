using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    [ProducesResponseType(typeof(List<FuelStationData>), 200)]
    public async Task<IActionResult> GetFiles()
    {
        return Ok(await client.GetFuelStations());
      
    }

    
}