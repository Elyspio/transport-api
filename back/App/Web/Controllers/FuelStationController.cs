using Abstractions.Interfaces.Services;
using Abstractions.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    [HttpGet("near")]
    [ProducesResponseType(typeof(List<FuelStationDataDistance>), 200)]
    public async Task<IActionResult> GetFuelStationsNear([Required] double latitude, [Required] double longitude, long radius = 10)
    {
        return Ok(await client.GetFuelStations(latitude, longitude, radius));

    }


    /// <summary>
    /// Get All fuel stations around a point
    /// </summary>
    [HttpGet("time")]
    [ProducesResponseType(typeof(List<FuelStationData>), 200)]
    public async Task<IActionResult> GetFuelStationsBetweenDates(DateTime? minDate = null, DateTime? maxDate = null)
    {
        return Ok(await client.GetBetweenDates(minDate, maxDate));

    }




    private List<FuelStationHistory> stations = new();

    /// <summary>
    /// Get All fuel stations history
    /// </summary>
    [HttpGet("fetch")]
    [ProducesResponseType(typeof(List<FuelStationHistory>), 200)]
    public async Task<IActionResult> Fetch()
    {

        if (stations.Count == 0)
        {
            // deserialize JSON directly from a file
            StreamReader file = System.IO.File.OpenText(@"P:\own\mobile\transport-api\back\Web\merged.json");
            var serializer = new JsonSerializer();
            stations = (List<FuelStationHistory>)serializer.Deserialize(file, typeof(List<FuelStationHistory>));

            file.Dispose();

            file = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();


        }




        var ids = stations.Select(s => s.Id).ToList();
        ids.Sort();


        return Ok(ids);

    }
}