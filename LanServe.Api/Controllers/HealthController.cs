using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using LanServe.Infrastructure.Data;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] IOptions<MongoOptions> opt)
    {
        return Ok(new
        {
            ok = true,
            dbConfigured = !string.IsNullOrWhiteSpace(opt.Value.ConnectionString) && !string.IsNullOrWhiteSpace(opt.Value.DbName),
            db = opt.Value.DbName
        });
    }
}
