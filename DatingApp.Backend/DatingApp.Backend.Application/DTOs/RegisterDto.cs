using System.ComponentModel.DataAnnotations;

namespace DatingApp.Backend.Application.DTOs;

public class RegisterDto
{
    [Required] public string UserName { get; set; }

    [Required] public string Password { get; set; }
}
