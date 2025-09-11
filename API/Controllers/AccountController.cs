using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
        this._context = context;
        this._tokenService = tokenService;
        this._mapper = mapper;

    }
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.UserName)) return BadRequest("User name is taken already");
        using var hmac = new HMACSHA512();
        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.UserName.ToLower();
        user.PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PassWordSalt = hmac.Key;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Gender = user.Gender,
            KnownAs = user.KnownAs
        };

    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
        .Include(x => x.Photos)
        .FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == loginDto.UserName.ToLower().Trim());
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
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            PhotoUrl = user.Photos?.FirstOrDefault(x => x.IsMain)?.Url
        };

    }
    private async Task<bool> UserExists(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower().Trim() == userName.ToLower().Trim());

    }

}
