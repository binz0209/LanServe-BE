using LanServe.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LanServe.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagController : ControllerBase
{
    private readonly MongoDbContext _db;
    private readonly IOptions<MongoOptions> _opt;

    public DiagController(MongoDbContext db, IOptions<MongoOptions> opt)
    {
        _db = db; _opt = opt;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            // Ping Mongo
            var cmd = new BsonDocument("ping", 1);
            await _db.Database.RunCommandAsync<BsonDocument>(cmd);

            return Ok(new
            {
                mongo = "OK",
                dbName = _opt.Value.DbName,              // phải KHỚP DbName
                hasUsersCollection = _db.Users != null,
                time = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            // trả về chi tiết để bạn thấy lỗi gì (tạm thời cho debug)
            return Problem(title: ex.GetType().Name, detail: ex.Message, statusCode: 500);
        }
    }
}
