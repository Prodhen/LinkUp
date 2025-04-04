using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;
    public AccountController(DataContext context)
    {
        this._context = context;

    }
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.UserName)) return BadRequest("User name is taken already");
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerDto.UserName,
            PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PassWordSalt = hmac.Key
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;

    }
    private async Task<bool> UserExists(string userName)
    {
        return await _context.Users.AnyAsync(x => x.UserName.ToLower().Trim() == userName.ToLower().Trim());

    }

}
