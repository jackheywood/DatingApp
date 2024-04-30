using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Application.Contracts.Persistence.Repositories;
using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.DTOs.Identity;
using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Infrastructure.Identity;

public class IdentityService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
    : IIdentityService
{
    public async Task<UserDto> RegisterUserAsync(RegisterDto registerDto)
    {
        var user = mapper.Map<AppUser>(registerDto);

        using var hmac = new HMACSHA512();

        user.Username = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;

        await userRepository.AddAsync(user);
        await userRepository.SaveAllAsync();

        return new UserDto
        {
            Username = user.Username,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            Gender = user.Gender,
        };
    }

    public async Task<UserDto> AuthenticateUserAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetByUsernameAsync(loginDto.Username);

        if (!IsPasswordValid(loginDto.Password, user)) return null;

        return new UserDto
        {
            Username = user.Username,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
            Gender = user.Gender,
        };
    }

    private static bool IsPasswordValid(string password, AppUser user)
    {
        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return !computedHash.Where((t, i) => t != user.PasswordHash[i]).Any();
    }
}
