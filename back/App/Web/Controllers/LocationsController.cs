using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Web.Controllers;

[Route("api/locations")]
[ApiController]
public class LocationsController : ControllerBase
{
	private readonly IDatabaseUpdateService databaseUpdateService;

	private readonly ILocationService locationService;

	public LocationsController(ILocationService locationService, IDatabaseUpdateService databaseUpdateService)
	{
		this.locationService = locationService;
		this.databaseUpdateService = databaseUpdateService;
	}

	[HttpGet]
	[SwaggerResponse(200, Type = typeof(List<Region>))]
	public async Task<IActionResult> GetAll()
	{
		return Ok(await locationService.GetMergedData());
	}


	[HttpPatch("regions")]
	[SwaggerResponse(204)]
	public async Task<IActionResult> Refresh()
	{
		await databaseUpdateService.RefreshLocations();
		return NoContent();
	}
}