using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace API.DTOs;

public class RegisterDto
{
    [Required]
    public required string UserName { get; set; }
    public required string Password { get; set; }
    public IFormFile? Picture { get; set; }

}
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one number")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.Picture)
            .Must(file => file == null || file.Length <= 5 * 1024 * 1024)
            .WithMessage("Profile picture must be less than 5MB")
            .Must(file => file == null ||
                new[] { ".jpg", ".jpeg", ".png", ".gif" }
                .Contains(Path.GetExtension(file.FileName).ToLower()))
            .WithMessage("Only JPG, JPEG, PNG, and GIF images are allowed");
    }
}
