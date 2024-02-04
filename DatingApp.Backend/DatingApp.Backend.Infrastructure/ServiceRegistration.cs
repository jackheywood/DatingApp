using System.Text;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Infrastructure.Data;
using DatingApp.Backend.Infrastructure.Data.Repositories;
using DatingApp.Backend.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Backend.Infrastructure;

public static class ServiceRegistration
{
    public static void AddDataServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DatingAppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        var key = Encoding.UTF8.GetBytes(config["TokenKey"]!);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
    }
}
