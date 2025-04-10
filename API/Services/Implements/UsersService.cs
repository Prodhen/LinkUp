using System;
using System.Security.Cryptography;
using System.Text;
using API.Common;
using API.Data.UnitOfWork;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Interface;
using API.Services.Interface;

namespace API.Services.Implements;

public class UserService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private ITokenService _tokenService;
    public UserService(IUnitOfWork unitOfWork, ITokenService tokenService   )
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
    }

    public async Task<ResponseDto> AddUser(UserAddDto userAddDto)
    {
        var userId = _unitOfWork.LoggedInUserId();

      //  if (await UserExists(registerDto.UserName)) return BadRequest("User name is taken already");


        string uniqueFileName = await ImageHelper.ProcessPicture(userAddDto.Picture);


        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = userAddDto.UserName,
            PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userAddDto.Password)),
            PassWordSalt = hmac.Key,
            PicturePath = uniqueFileName
        };
         _unitOfWork.Users.Add(user);

        await _unitOfWork.SaveAsync();

        var response= new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Picture = user.PicturePath
        };
    
        
 

        return Utilities.SuccessResponseForAdd(response);

    }

}
