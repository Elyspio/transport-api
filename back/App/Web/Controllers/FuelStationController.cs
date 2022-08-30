using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Common.Helpers;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Web.Controllers;

[ApiController]
[Route("api/fuel-stations", Name = "FuelStations")]
public class FuelStationController : ControllerBase
{
	private readonly IFuelStationService client;
	private readonly ILogger<FuelStationLocation> logger;

	public FuelStationController(IFuelStationService client, ILogger<FuelStationLocation> logger)
	{
		this.client = client;
		this.logger = logger;
	}

	/// <summary>
	///     Get All fuel stations around a point
	/// </summary>
	/// <param name="latitude"></param>
	/// <param name="longitude"></param>
	/// <param name="radius">Maximal distance in meter between the fuel station and the specified point</param>
	[HttpGet("near")]
	[ProducesResponseType(typeof(List<FuelStationDataDistance>), 200)]
	public async Task<IActionResult> GetFuelStationsNear([Required] double latitude, [Required] double longitude, long radius = 10)
	{
		var log = logger.Enter($"{Log.Format(latitude)} {Log.Format(longitude)} {Log.Format(radius)}");
		try
		{
			return Ok(await client.GetFuelStations(latitude, longitude, radius));
		}
		finally
		{
			log.Exit();
		}
	}


	/// <summary>
	///     Get All fuel stations data between dates
	/// </summary>
	[HttpGet("history")]
	[ProducesResponseType(typeof(List<FuelStationData>), 200)]
	public async Task<IActionResult> GetFuelStationsBetweenDates(DateTime? minDate = null, DateTime? maxDate = null)
	{
		var log = logger.Enter($"{Log.Format(minDate)} {Log.Format(maxDate)}");
		try
		{
			return Ok(await client.GetBetweenDates(minDate, maxDate));
		}
		finally
		{
			log.Exit();
		}
	}
}