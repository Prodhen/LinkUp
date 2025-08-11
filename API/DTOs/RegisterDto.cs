using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]

    public string UserName { get; set; } = string.Empty;
    [StringLength(8, MinimumLength = 4)]
    public string Password { get; set; } = string.Empty;

}
