using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Application.Services;
using LanServe.Infrastructure.Data;
using LanServe.Infrastructure.Repositories;
using LanServe.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Controllers
services.AddControllers();

// Swagger (OpenAPI cho .NET 8)
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// CORS: cho FE Vite chạy http://localhost:5173
const string CorsPolicy = "LanServeCors";
services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p => p
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// MongoDbContext (singleton để share connection)
services.AddSingleton<MongoDbContext>();

// Repositories
services.AddScoped<IUserRepository>(sp =>
    new UserRepository(sp.GetRequiredService<MongoDbContext>().Users));
services.AddScoped<IUserProfileRepository>(sp =>
    new UserProfileRepository(sp.GetRequiredService<MongoDbContext>().UserProfiles));
services.AddScoped<ICategoryRepository>(sp =>
    new CategoryRepository(sp.GetRequiredService<MongoDbContext>().Categories));
services.AddScoped<ISkillRepository>(sp =>
    new SkillRepository(sp.GetRequiredService<MongoDbContext>().Skills));
services.AddScoped<IProjectRepository>(sp =>
    new ProjectRepository(sp.GetRequiredService<MongoDbContext>().Projects));
services.AddScoped<IProjectSkillRepository>(sp =>
    new ProjectSkillRepository(sp.GetRequiredService<MongoDbContext>().ProjectSkills));
services.AddScoped<IProposalRepository>(sp =>
    new ProposalRepository(sp.GetRequiredService<MongoDbContext>().Proposals));
services.AddScoped<IContractRepository>(sp =>
    new ContractRepository(sp.GetRequiredService<MongoDbContext>().Contracts));
services.AddScoped<IPaymentRepository>(sp =>
    new PaymentRepository(sp.GetRequiredService<MongoDbContext>().Payments));
services.AddScoped<IMessageRepository>(sp =>
    new MessageRepository(sp.GetRequiredService<MongoDbContext>().Messages));
services.AddScoped<INotificationRepository>(sp =>
    new NotificationRepository(sp.GetRequiredService<MongoDbContext>().Notifications));
services.AddScoped<IReviewRepository>(sp =>
    new ReviewRepository(sp.GetRequiredService<MongoDbContext>().Reviews));

// Services (Application)
services.AddScoped<IUserService, UserService>();
services.AddScoped<IUserProfileService, UserProfileService>();
services.AddScoped<ICategoryService, CategoryService>();
services.AddScoped<ISkillService, SkillService>();
services.AddScoped<IProjectService, ProjectService>();
services.AddScoped<IProjectSkillService, ProjectSkillService>();
services.AddScoped<IProposalService, ProposalService>();
services.AddScoped<IContractService, ContractService>();
services.AddScoped<IPaymentService, PaymentService>();
services.AddScoped<IMessageService, MessageService>();
services.AddScoped<INotificationService, NotificationService>();
services.AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

// Swagger UI (bật ở Dev hoặc tuỳ ý)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);

app.MapControllers();
app.Run();
