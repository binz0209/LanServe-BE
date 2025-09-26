// LanServe.Infrastructure/Data/MongoDbContext.cs
using LanServe.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LanServe.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _db;
    private readonly MongoOptions _opt;

    public MongoDbContext(IOptions<MongoOptions> options)
    {
        _opt = options.Value;
        var client = new MongoClient(_opt.ConnectionString);
        _db = client.GetDatabase(_opt.Database);
    }

    public IMongoCollection<User> Users => _db.GetCollection<User>(_opt.Coll.Users);
    public IMongoCollection<UserProfile> UserProfiles => _db.GetCollection<UserProfile>(_opt.Coll.UserProfiles);
    public IMongoCollection<Category> Categories => _db.GetCollection<Category>(_opt.Coll.Categories);
    public IMongoCollection<Skill> Skills => _db.GetCollection<Skill>(_opt.Coll.Skills);
    public IMongoCollection<Project> Projects => _db.GetCollection<Project>(_opt.Coll.Projects);
    public IMongoCollection<ProjectSkill> ProjectSkills => _db.GetCollection<ProjectSkill>(_opt.Coll.ProjectSkills);
    public IMongoCollection<Proposal> Proposals => _db.GetCollection<Proposal>(_opt.Coll.Proposals);
    public IMongoCollection<Contract> Contracts => _db.GetCollection<Contract>(_opt.Coll.Contracts);
    public IMongoCollection<Message> Messages => _db.GetCollection<Message>(_opt.Coll.Messages);
    public IMongoCollection<Notification> Notifications => _db.GetCollection<Notification>(_opt.Coll.Notifications);
    public IMongoCollection<Review> Reviews => _db.GetCollection<Review>(_opt.Coll.Reviews);
    public IMongoCollection<Payment> Payments => _db.GetCollection<Payment>(_opt.Coll.Payments);

    public async Task EnsureIndexesAsync()
    {
        // Users
        await Users.Indexes.CreateOneAsync(new CreateIndexModel<User>(
            Builders<User>.IndexKeys.Ascending(x => x.Email),
            new CreateIndexOptions { Unique = true }));

        // Profiles
        await UserProfiles.Indexes.CreateOneAsync(new CreateIndexModel<UserProfile>(
            Builders<UserProfile>.IndexKeys.Ascending(x => x.UserId),
            new CreateIndexOptions { Unique = true }));

        // Projects
        await Projects.Indexes.CreateOneAsync(new CreateIndexModel<Project>(
            Builders<Project>.IndexKeys.Ascending(x => x.OwnerId).Descending(x => x.CreatedAt)));
        await Projects.Indexes.CreateOneAsync(new CreateIndexModel<Project>(
            Builders<Project>.IndexKeys.Ascending(x => x.CategoryId)));

        // ProjectSkills (nếu dùng)
        await ProjectSkills.Indexes.CreateOneAsync(new CreateIndexModel<ProjectSkill>(
            Builders<ProjectSkill>.IndexKeys.Ascending(x => x.ProjectId).Ascending(x => x.SkillId),
            new CreateIndexOptions { Unique = true }));

        // Proposals
        await Proposals.Indexes.CreateOneAsync(new CreateIndexModel<Proposal>(
            Builders<Proposal>.IndexKeys.Ascending(x => x.ProjectId).Ascending(x => x.FreelancerId),
            new CreateIndexOptions { Unique = true }));

        // Contracts
        await Contracts.Indexes.CreateOneAsync(new CreateIndexModel<Contract>(
            Builders<Contract>.IndexKeys.Ascending(x => x.ProjectId), new CreateIndexOptions { Unique = true }));

        // Messages
        await Messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(
            Builders<Message>.IndexKeys.Ascending(x => x.ConversationKey).Ascending(x => x.CreatedAt)));
        await Messages.Indexes.CreateOneAsync(new CreateIndexModel<Message>(
            Builders<Message>.IndexKeys.Ascending(x => x.SenderId).Ascending(x => x.ReceiverId)));

        // Notifications
        await Notifications.Indexes.CreateOneAsync(new CreateIndexModel<Notification>(
            Builders<Notification>.IndexKeys.Ascending(x => x.UserId).Descending(x => x.CreatedAt)));

        // Reviews
        await Reviews.Indexes.CreateOneAsync(new CreateIndexModel<Review>(
            Builders<Review>.IndexKeys.Ascending(x => x.ProjectId).Ascending(x => x.RevieweeId)));

        // Payments
        await Payments.Indexes.CreateOneAsync(new CreateIndexModel<Payment>(
            Builders<Payment>.IndexKeys.Ascending(x => x.ContractId)));
    }
}
