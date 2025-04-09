using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interface;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config)
    {
        this._config = config;

    }
    public string CreateToken(AppUser user)
    {
        var tokenKey = _config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appSetting ");
        if (tokenKey.Length < 64) throw new Exception("Your token needs to be longer");
        var key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var claims=new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.Id) ),
            new Claim(ClaimTypes.Name,user.UserName)
        };
        var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor=new SecurityTokenDescriptor{
            Subject=new ClaimsIdentity(claims),
            Expires=DateTime.UtcNow.AddDays(1),
            SigningCredentials=creds
        };
        var tokenHandler=new JwtSecurityTokenHandler();
        var token=tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
