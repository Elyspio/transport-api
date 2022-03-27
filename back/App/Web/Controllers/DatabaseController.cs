using Microsoft.AspNetCore.Mvc;

namespace Transport.Api.Web.Controllers;

[Route("api/database/operation")]
[ApiController]
public class DatabaseController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UpdateYearly(int year)
    {
        return NoContent();
    }
}