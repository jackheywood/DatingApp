using System.Security.Cryptography;
using System.Text;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.DTOs.Identity;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Infrastructure.Identity;

public class IdentityService(IUserRepository userRepository, ITokenService tokenService) : IIdentityService
{
    public async Task<AppUser> RegisterUserAsync(RegisterDto registerDto)
    {
        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDto.UserName.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key,
        };

        return await userRepository.AddAsync(user);
    }

    public async Task<UserDto> AuthenticateUserAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetByUserNameAsync(loginDto.UserName);

        if (!IsPasswordValid(loginDto.Password, user)) return null;

        return new UserDto
        {
            UserName = user.UserName,
            Token = tokenService.CreateToken(user),
        };
    }

    private static bool IsPasswordValid(string password, AppUser user)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != user.PasswordHash[i]).Any();
    }
}
