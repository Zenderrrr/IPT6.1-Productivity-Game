using FocusUp.Application.Services;
using FocusUp.Application.Services.Auth;
using FocusUp.Application.Strategies;
using FocusUp.Application.Strategies.Interfaces;
using FocusUp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton(DatabaseConnection.GetInstance(connectionString));
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<BadgeService>();
builder.Services.AddScoped<LevelService>();
builder.Services.AddScoped<StreakService>();
builder.Services.AddScoped<TaskCompletionService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<XPService>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IStreakRuleStrategy, DefaultStreakRuleStrategy>();
builder.Services.AddScoped<IXpCalculationStrategy, DefaultXpCalculationStrategy>();
builder.Services.AddScoped<ILevelStrategy, QuadraticLevelStrategy>();
builder.Services.AddScoped<IBadgeRule, StreakBadgeRule>();

builder.Services.AddScoped<BadgeRepository>();
builder.Services.AddScoped<TaskLogRepository>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<UserBadgeRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserStatsRepository>();
builder.Services.AddScoped<XPEventRepository>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }
    );

builder.Services.AddAuthentication();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
