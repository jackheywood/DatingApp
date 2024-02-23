using System.Security.Cryptography;
using System.Text.Json;
using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence;

public class Seed
{
    public static async Task SeedUsers(DatingAppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("../DatingApp.Backend.Infrastructure/Data/UserSeedData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();
            user.Username = user.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash("Pa$$w0rd"u8.ToArray());
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
