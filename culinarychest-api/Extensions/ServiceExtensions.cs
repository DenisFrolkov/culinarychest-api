using System.Text;
using Contracts;
using Entities;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;

namespace culinarychest_api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) 
        => services.AddCors(options => 
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyOrigin() 
                    .AllowAnyMethod() 
                    .AllowAnyHeader()); 
        }
    );

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["validIssuer"],
                        ValidAudience = jwtSettings["validAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };
            });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<ApplicationUser>(o =>
        {
            o.Password.RequireDigit = true; o.Password.RequireLowercase = false; o.Password.RequireUppercase = false; o.Password.RequireNonAlphanumeric = false; o.Password.RequiredLength = 10; o.User.RequireUniqueEmail = true;
        });
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<RepositoryContext>() .AddDefaultTokenProviders();
    }
    
    public static void ConfigureIISIntegration(this IServiceCollection services)
        => services.Configure<IISOptions>(options => { }); 
    
    public static void ConfigureLoggerService(this IServiceCollection services) 
        => services.AddScoped<ILoggerManager, LoggerManager>(); 
    
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) => 
        services.AddDbContext<RepositoryContext>(opts => 
            opts.UseNpgsql(configuration.GetConnectionString("sqlConnection"), b => 
                b.MigrationsAssembly("culinarychest-api"))); 

    public static void ConfigureRepositoryManager(this IServiceCollection services) => 
        services.AddScoped<IRepositoryManager, RepositoryManager>();

}