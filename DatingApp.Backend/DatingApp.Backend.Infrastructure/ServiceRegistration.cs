﻿using System.Text;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.Contracts.Persistence.Services;
using DatingApp.Backend.Infrastructure.Helpers;
using DatingApp.Backend.Infrastructure.Identity;
using DatingApp.Backend.Infrastructure.Persistence;
using DatingApp.Backend.Infrastructure.Persistence.Repositories;
using DatingApp.Backend.Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Backend.Infrastructure;

public static class ServiceRegistration
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DatingAppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILikesRepository, LikesRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();

        services.AddScoped<IPhotoService, PhotoService>();
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