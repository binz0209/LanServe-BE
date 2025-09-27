using System.Text;
using LanServe.Application.Interfaces.Repositories;
using LanServe.Application.Interfaces.Services;
using LanServe.Application.Services;
using LanServe.Infrastructure.Data;
using LanServe.Infrastructure.Repositories;
using LanServe.Infrastructure.Services;
using LanServe.Infrastructure.Initialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// ========== Mongo Options + DbContext ==========
services.Configure<MongoOptions>(config.GetSection("MongoDb"));
services.AddSingleton<MongoDbContext>();

// Initializer (tạo DB/collection/index khi app start)
services.AddSingleton<IMongoInitializer, MongoInitializer>();
services.AddHostedService<MongoInitializerHostedService>();

// ========== Controllers ==========
services.AddControllers();

// ========== Swagger ==========
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LanServe API", Version = "v1" });

    // JWT on Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter 'Bearer {token}'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// ========== CORS (FE Vite) ==========
const string CorsPolicy = "LanServeCors";
services.AddCors(opt =>
{
    opt.AddPolicy(CorsPolicy, p => p
        .WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

// ========== JWT Authentication ==========
var jwtKey = config["Jwt:Key"];
if (!string.IsNullOrWhiteSpace(jwtKey))
{
    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // dev
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true
            };
        });
}

// ========== Repositories ==========
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

// ========== Services (Application) ==========
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

// ========== Middlewares ==========
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LanServe API v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors(CorsPolicy);

if (!string.IsNullOrWhiteSpace(jwtKey))
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

app.Run();
