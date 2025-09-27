using LanServe.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Initialization;

public class MongoInitializer : IMongoInitializer
{
    private readonly MongoDbContext _ctx;
    private readonly MongoOptions _opt;
    private readonly ILogger<MongoInitializer> _logger;

    public MongoInitializer(MongoDbContext ctx, IOptions<MongoOptions> opt, ILogger<MongoInitializer> logger)
    {
        _ctx = ctx;
        _opt = opt.Value;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken ct = default)
    {
        // 1) Ping để chắc kết nối ok
        await _ctx.Database.RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }", cancellationToken: ct);
        _logger.LogInformation("Mongo ping OK on {Db}", _opt.DbName);

        // 2) Tạo collections nếu chưa có
        var existing = await _ctx.Database.ListCollectionNames().ToListAsync(ct);
        async Task EnsureCollection(string name)
        {
            if (!existing.Contains(name))
            {
                await _ctx.Database.CreateCollectionAsync(name, cancellationToken: ct);
                _logger.LogInformation("Created collection: {Name}", name);
            }
        }

        await EnsureCollection(_opt.Collections.Users);
        await EnsureCollection(_opt.Collections.UserProfiles);
        await EnsureCollection(_opt.Collections.Projects);
        await EnsureCollection(_opt.Collections.Proposals);
        await EnsureCollection(_opt.Collections.Contracts);
        await EnsureCollection(_opt.Collections.Payments);
        await EnsureCollection(_opt.Collections.Reviews);
        await EnsureCollection(_opt.Collections.Messages);
        await EnsureCollection(_opt.Collections.Notifications);
        await EnsureCollection(_opt.Collections.Categories);
        await EnsureCollection(_opt.Collections.Skills);

        // 3) Indexes tối thiểu
        // Users: unique email
        var users = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.Users);
        await users.Indexes.CreateOneAsync(
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("email"),
                new CreateIndexOptions { Unique = true, Name = "ux_users_email" }
            ), cancellationToken: ct);

        // Projects: title text, category, deadline
        var projects = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.Projects);
        await projects.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Text("title").Text("description"),
                new CreateIndexOptions { Name = "tx_projects_title_desc" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("category"),
                new CreateIndexOptions { Name = "ix_projects_category" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("deadline"),
                new CreateIndexOptions { Name = "ix_projects_deadline" })
        }, cancellationToken: ct);

        _logger.LogInformation("Mongo initialization finished for {Db}", _opt.DbName);
    }
}
