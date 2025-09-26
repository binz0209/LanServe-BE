using LanServe.Infrastructure.Data;
using LanServe.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký OpenAPI (Swagger UI + OpenAPI 3)
builder.Services.AddOpenApi();

// Đăng ký MongoDbContext (singleton để chia sẻ kết nối)
builder.Services.AddSingleton<MongoDbContext>();

// Đăng ký repositories
builder.Services.AddScoped<IUserRepository>(sp =>
    new UserRepository(sp.GetRequiredService<MongoDbContext>().Users));
builder.Services.AddScoped<IUserProfileRepository>(sp =>
    new UserProfileRepository(sp.GetRequiredService<MongoDbContext>().UserProfiles));
builder.Services.AddScoped<ICategoryRepository>(sp =>
    new CategoryRepository(sp.GetRequiredService<MongoDbContext>().Categories));
builder.Services.AddScoped<ISkillRepository>(sp =>
    new SkillRepository(sp.GetRequiredService<MongoDbContext>().Skills));
builder.Services.AddScoped<IProjectRepository>(sp =>
    new ProjectRepository(sp.GetRequiredService<MongoDbContext>().Projects));
builder.Services.AddScoped<IProjectSkillRepository>(sp =>
    new ProjectSkillRepository(sp.GetRequiredService<MongoDbContext>().ProjectSkills));
builder.Services.AddScoped<IProposalRepository>(sp =>
    new ProposalRepository(sp.GetRequiredService<MongoDbContext>().Proposals));
builder.Services.AddScoped<IContractRepository>(sp =>
    new ContractRepository(sp.GetRequiredService<MongoDbContext>().Contracts));
builder.Services.AddScoped<IPaymentRepository>(sp =>
    new PaymentRepository(sp.GetRequiredService<MongoDbContext>().Payments));
builder.Services.AddScoped<IMessageRepository>(sp =>
    new MessageRepository(sp.GetRequiredService<MongoDbContext>().Messages));
builder.Services.AddScoped<INotificationRepository>(sp =>
    new NotificationRepository(sp.GetRequiredService<MongoDbContext>().Notifications));
builder.Services.AddScoped<IReviewRepository>(sp =>
    new ReviewRepository(sp.GetRequiredService<MongoDbContext>().Reviews));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISkillService, SkillService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectSkillService, ProjectSkillService>();
builder.Services.AddScoped<IProposalService, ProposalService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IReviewService, ReviewService>();



var app = builder.Build();

// Cấu hình middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Mở UI cho swagger.json
}

app.UseHttpsRedirection();

// Map controllers (sau này bạn tạo controller thì sẽ tự expose API)
app.MapControllers();

app.Run();
