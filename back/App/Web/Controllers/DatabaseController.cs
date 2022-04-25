using Microsoft.AspNetCore.Mvc;
using Transport.Api.Abstractions.Interfaces.Services;

namespace Transport.Api.Web.Controllers;

[Route("api/database")]
[ApiController]
public class DatabaseController : ControllerBase
{
	private readonly IDatabaseUpdateService databaseUpdateService;

	public DatabaseController(IDatabaseUpdateService databaseUpdateService)
	{
		this.databaseUpdateService = databaseUpdateService;
	}

	[HttpPut("prices/{year:int}/refresh")]
	public async Task<IActionResult> UpdateYearly(int year)
	{
		await databaseUpdateService.RefreshYearly(year);
		return NoContent();
	}
}