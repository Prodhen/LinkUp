using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public IFormFile? Picture { get; set; }

}
