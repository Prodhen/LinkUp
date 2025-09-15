using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> userManager;
    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper, DataContext context)
    {
        this._context = context;
        this._tokenService = tokenService;
        this._mapper = mapper;
        this.userManager = userManager;

    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) return BadRequest("User name is taken already");
        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();
        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
            Gender = user.Gender,
            KnownAs = user.KnownAs
        };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
        .Include(x => x.Photos)
        .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDto.UserName.ToUpper());
        if (user == null || user.UserName == null) return Unauthorized("Invalid username");
        return new UserDto
        {
            UserName = user.UserName,
            Token = await _tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
        };

    }
    private async Task<bool> UserExists(string userName)
    {
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == userName.ToUpper()); // Bob != bob

    }

}
