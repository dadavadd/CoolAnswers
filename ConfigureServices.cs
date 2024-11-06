using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using CoolAnswers.Repositories.Interfaces;
using CoolAnswers.Repositories;
using CoolAnswers.Services.Interfaces;
using CoolAnswers.Services;
using CoolAnswers.Models;
using CoolAnswers.Data;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace CoolAnswers;

public static class ConfigureServices
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite($"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.Configuration.GetConnectionString("DefaultConnection"))}"));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IJwtService, JwtService>();
        builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
        builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
        builder.Services.AddScoped<IComplaintRepository, ComplaintRepository>();
        builder.Services.AddScoped<IRankService, RankService>();

        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 12;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddAuthenticationService(this WebApplicationBuilder builder)
    {

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                ValidAudience = builder.Configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["userkey"];
                    return Task.CompletedTask;
                }
            };
        });
    }
}
