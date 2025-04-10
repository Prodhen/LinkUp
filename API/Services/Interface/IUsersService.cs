using System;
using API.Common;
using API.DTOs;

namespace API.Services.Interface;

public interface IUsersService
{
    Task<ResponseDto> Register(RegisterDto registerDto);
    Task<ResponseDto> login(LoginDto dto);

}
