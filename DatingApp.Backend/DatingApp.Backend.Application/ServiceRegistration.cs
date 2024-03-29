﻿using DatingApp.Backend.Application.Contracts.Services;
using DatingApp.Backend.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Backend.Application;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IUserService, UserService>();
    }
}
