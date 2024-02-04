using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Application.DTOs.Identity;

public class LoginDto
{
    [Required] public string Username { get; set; }
    [Required] public string Password { get; set; }
}
