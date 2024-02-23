using DatingApp.Backend.Application.DTOs;
using DatingApp.Backend.Application.DTOs.Identity;

namespace DatingApp.Backend.Application.Contracts.Identity;

public interface IIdentityService
{
    Task<UserDto> RegisterUserAsync(RegisterDto registerDto);
    Task<UserDto> AuthenticateUserAsync(LoginDto loginDto);
}
