using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Controllers;
public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IWebHostEnvironment _environment;
    public AccountController(DataContext context, ITokenService tokenService, IWebHostEnvironment environment)
    {
        this._context = context;
        this._tokenService = tokenService;
        this._environment = environment;

    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) return BadRequest("User name is taken already");

        if (registerDto.Picture == null || registerDto.Picture.Length == 0)
        {
            return BadRequest("Please upload a picture.");
        }

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{registerDto.Picture.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await registerDto.Picture.CopyToAsync(stream);
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "Failed to save the uploaded picture.");
        }



        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerDto.UserName,
            PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PassWordSalt = hmac.Key,
            PicturePath = $"/uploads/{uniqueFileName}"
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Picture = user.PicturePath
        };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == loginDto.UserName.ToLower().Trim());
        if (user == null) return Unauthorized("Invalid User Name.");
        using var hmac = new HMACSHA512(user.PassWordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PassWordHash[i]) return Unauthorized("Invalid password");
        }

        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Picture = user.PicturePath,
        };

    }
    private async Task<bool> UserExists(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower().Trim() == userName.ToLower().Trim());

    }



}
