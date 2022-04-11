using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Web.Controllers;

[Route("api/statistics")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IStatsService statsService;

    public StatisticsController(IStatsService databaseUpdateService)
    {
        statsService = databaseUpdateService;
    }

    [HttpPatch("refresh")]
    [SwaggerResponse(204)]
    public async Task<IActionResult> Refresh()
    {
        await statsService.RefreshStats();
        return NoContent();
    }


    [HttpPatch("refresh/weekly/")]
    [SwaggerResponse(204)]
    public async Task<IActionResult> RefreshWeeklyStats([Required] bool clear = true, int? year = null)
    {
        await statsService.RefreshWeeklyStats(clear, year);
        return NoContent();
    }


    [HttpPatch("refresh/daily")]
    [SwaggerResponse(204)]
    public async Task<IActionResult> RefreshDailyStats([Required] bool clear = true)
    {
        await statsService.RefreshDailyStats(clear);
        return NoContent();
    }


    [HttpGet("{statsTimeType}")]
    [SwaggerResponse(200, Type = typeof(List<Statistic>))]
    public async Task<IActionResult> GetWeeklyStats([Required] StatsTimeType statsTimeType)
    {
        return Ok(await statsService.GetWeeklyStats(statsTimeType));
    }

}