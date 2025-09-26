// LanServe.Infrastructure/Data/MongoOptions.cs
namespace LanServe.Infrastructure.Data;

public class MongoOptions
{
    public string ConnectionString { get; set; } = null!;
    public string Database { get; set; } = null!;
    public Collections Coll { get; set; } = new();

    public class Collections
    {
        public string Users { get; set; } = "users";
        public string UserProfiles { get; set; } = "user_profiles";
        public string Categories { get; set; } = "categories";
        public string Skills { get; set; } = "skills";
        public string Projects { get; set; } = "projects";
        public string ProjectSkills { get; set; } = "project_skills";
        public string Proposals { get; set; } = "proposals";
        public string Contracts { get; set; } = "contracts";
        public string Messages { get; set; } = "messages";
        public string Notifications { get; set; } = "notifications";
        public string Reviews { get; set; } = "reviews";
        public string Payments { get; set; } = "payments";
    }
}
