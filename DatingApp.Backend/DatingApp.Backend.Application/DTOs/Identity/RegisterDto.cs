using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Application.DTOs.Identity;

public class RegisterDto
{
    [Required] public string Username { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 4)]
    public string Password { get; set; }
}
