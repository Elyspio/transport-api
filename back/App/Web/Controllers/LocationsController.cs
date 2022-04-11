using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationsController : ControllerBase
{

    private readonly ILocationService locationService;
    private readonly IDatabaseUpdateService databaseUpdateService;
    public LocationsController(ILocationService locationService, IDatabaseUpdateService databaseUpdateService)
    {
        this.locationService = locationService;
        this.databaseUpdateService = databaseUpdateService;
    }

    [HttpGet("regions/{region}/departements")]
    [SwaggerResponse(200, Type = typeof(List<Departement>))]
    public async Task<IActionResult> GetDepartementsByRegion(Region region)
    {
        return Ok(await locationService.GetDepartements(region));
    }


    [HttpGet("departements")]
    [SwaggerResponse(200, Type = typeof(List<Departement>))]
    public async Task<IActionResult> GetAllDepartements()
    {
        return Ok(await locationService.GetAllDepartements());
    }



    [HttpGet("regions")]
    [SwaggerResponse(200, Type = typeof(List<RegionTransport>))]
    public async Task<IActionResult> GetRegions()
    {
        return Ok(await locationService.GetRegions());
    }


    [HttpPatch("regions")]
    [SwaggerResponse(204)]

    public async Task<IActionResult> Refresh()
    {
        await databaseUpdateService.RefreshLocations();
        return NoContent();
    }
}