using FocusUp.Application.Services;
using FocusUp.Application.Services.Auth;
using FocusUp.Application.Strategies;
using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using static System.Threading.Tasks.Task;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton(DatabaseConnection.GetInstance(connectionString));
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddEndpointsApiExplorer();

const string schemeId = "Bearer";
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FocusUp API",
        Version = "v1"
    });

    options.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(schemeId),
            new List<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<BadgeService>();
builder.Services.AddScoped<LevelService>();
builder.Services.AddScoped<StreakService>();
builder.Services.AddScoped<TaskCompletionService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<XPService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StatsService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<TaskLogService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserStatsService>();
builder.Services.AddScoped<XpEventService>();

builder.Services.AddScoped<IStreakRuleStrategy, DefaultStreakRuleStrategy>();
builder.Services.AddScoped<IXpCalculationStrategy, DefaultXpCalculationStrategy>();
builder.Services.AddScoped<ILevelStrategy, QuadraticLevelStrategy>();
builder.Services.AddScoped<IBadgeRule, StreakBadgeRule>();
builder.Services.AddScoped<IBadgeRule, TasksCompletedBadgeRule>();
builder.Services.AddScoped<IBadgeRule, TimeLoggedBadgeRule>();
builder.Services.AddScoped<IBadgeRule, TotalXpBadgeRule>();

builder.Services.AddScoped<BadgeRepository>();
builder.Services.AddScoped<TaskLogRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<UserBadgeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserStatsRepository>();
builder.Services.AddScoped<XPEventRepository>();
builder.Services.AddScoped<CategoryRepository>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;

        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        //options.Events = new JwtBearerEvents
        //{
        //    OnAuthenticationFailed = context =>
        //    {
        //        Console.WriteLine("AUTH FAILED: " + context.Exception.ToString());
        //        return System.Threading.Tasks.Task.CompletedTask;
        //    },
        //    OnTokenValidated = context =>
        //    {
        //        Console.WriteLine("TOKEN VALID");
        //        return System.Threading.Tasks.Task.CompletedTask;
        //    }
        //};
    });

builder.Services.AddAuthorization();
var app = builder.Build();

//app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
