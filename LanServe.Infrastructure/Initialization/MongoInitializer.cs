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
        // 1) Ping
        await _ctx.Database.RunCommandAsync((Command<BsonDocument>)"{ ping: 1 }", cancellationToken: ct);
        _logger.LogInformation("Mongo ping OK on {Db}", _opt.DbName);

        // 2) Create collections if missing
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
        await EnsureCollection(_opt.Collections.ProjectSkills);   // 👈 THÊM DÒNG NÀY
        await EnsureCollection(_opt.Collections.Proposals);
        await EnsureCollection(_opt.Collections.Contracts);
        await EnsureCollection(_opt.Collections.Payments);
        await EnsureCollection(_opt.Collections.Reviews);
        await EnsureCollection(_opt.Collections.Messages);
        await EnsureCollection(_opt.Collections.Notifications);
        await EnsureCollection(_opt.Collections.Categories);
        await EnsureCollection(_opt.Collections.Skills);

        // 3) Minimal indexes

        // Users: unique email
        var users = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.Users);
        await users.Indexes.CreateOneAsync(
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("email"),
                new CreateIndexOptions { Unique = true, Name = "ux_users_email" }
            ), cancellationToken: ct);

        // Projects: text + fields commonly filtered
        var projects = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.Projects);
        await projects.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Text("title").Text("description"),
                new CreateIndexOptions { Name = "tx_projects_title_desc" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("categoryId"),
                new CreateIndexOptions { Name = "ix_projects_categoryId" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("status"),
                new CreateIndexOptions { Name = "ix_projects_status" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("ownerId"),
                new CreateIndexOptions { Name = "ix_projects_ownerId" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("createdAt"),
                new CreateIndexOptions { Name = "ix_projects_createdAt" })
        }, cancellationToken: ct);

        // ProjectSkills: tránh trùng 1 skill cho 1 project
        var projectSkills = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.ProjectSkills);
        await projectSkills.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("projectId").Ascending("skillId"),
                new CreateIndexOptions { Name = "ux_projectskills_projectId_skillId", Unique = true }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("projectId"),
                new CreateIndexOptions { Name = "ix_projectskills_projectId" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("skillId"),
                new CreateIndexOptions { Name = "ix_projectskills_skillId" })
        }, cancellationToken: ct);

        // Messages: tối ưu thread & user lookup (tuỳ chọn)
        var messages = _ctx.Database.GetCollection<BsonDocument>(_opt.Collections.Messages);
        await messages.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("conversationKey").Ascending("createdAt"),
                new CreateIndexOptions { Name = "ix_messages_conversation_created" }),
            new CreateIndexModel<BsonDocument>(
                Builders<BsonDocument>.IndexKeys.Ascending("senderId").Ascending("receiverId").Ascending("createdAt"),
                new CreateIndexOptions { Name = "ix_messages_participants_created" })
        }, cancellationToken: ct);

        _logger.LogInformation("Mongo initialization finished for {Db}", _opt.DbName);
    }
}
