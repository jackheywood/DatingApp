using DatingApp.Backend.Domain.Entities;

namespace DatingApp.Backend.Application.Contracts.Identity;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
