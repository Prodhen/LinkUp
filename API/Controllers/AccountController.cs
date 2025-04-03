using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AccountController:BaseApiController
{
    private readonly DataContext _context;
    public AccountController(DataContext context){
        this._context = context;

    }
    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(string username,string password){
        using var hmac=new HMACSHA512();
        var user= new AppUser{
            UserName=username,
            PassWordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
            PassWordSalt=hmac.Key
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;

    }

}
