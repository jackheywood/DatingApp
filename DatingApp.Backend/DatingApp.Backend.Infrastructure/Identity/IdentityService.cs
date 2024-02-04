using System.Security.Cryptography;
using System.Text;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Infrastructure.Identity;

public class IdentityService(IUserRepository userRepository) : IIdentityService
{
    public async Task<AppUser> RegisterUserAsync(RegisterDto registerDto)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        return await userRepository.AddAsync(user);
    }
}
