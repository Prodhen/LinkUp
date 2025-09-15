using System;
using API.Entities;

namespace API.Interface;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);

}
