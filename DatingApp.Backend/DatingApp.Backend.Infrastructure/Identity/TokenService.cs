using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DatingApp.Backend.Application.Contracts.Identity;
using DatingApp.Backend.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Backend.Infrastructure.Identity;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly SymmetricSecurityKey _key = new(Encoding.UTF8.GetBytes(config["TokenKey"]!));

    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.Username),
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
