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
using FluentValidation;
namespace API.Services.Implements;
public class UserService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    private ITokenService _tokenService;
    private IValidator<RegisterDto> _regValidator;
    public UserService(IUnitOfWork unitOfWork, ITokenService tokenService, IValidator<RegisterDto> regValidator)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _regValidator = regValidator;
    }
    public async Task<ResponseDto> Register(RegisterDto registerDto)
    {
        // var userId = _unitOfWork.LoggedInUserId();
        var validationResult = await _regValidator.ValidateAsync(registerDto);

        if (!validationResult.IsValid)
        {
            return Utilities.ValidationErrorResponse(ErrorHelper.FluentErrorMessages(validationResult.Errors));
        }

        if (await UserExists(registerDto.UserName)) return Utilities.ValidationErrorResponse("User name is taken already");
        string uniqueFileName = await ImageHelper.ProcessPicture(registerDto.Picture);
        using var hmac = new HMACSHA512();
        var user = new AppUser
        {
            UserName = registerDto.UserName,
            PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PassWordSalt = hmac.Key,
            PicturePath = uniqueFileName
        };
        _unitOfWork.Users.Add(user);

        await _unitOfWork.SaveAsync();

        var response = new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Picture = user.PicturePath
        };

        return Utilities.SuccessResponseForAdd(response);

    }

    public async Task<ResponseDto> login(LoginDto dto)
    {

        var user = await _unitOfWork.Users.GetWhere(x => x.UserName.ToLower().Trim() == dto.UserName.ToLower().Trim());
        if (user == null) return Utilities.ValidationErrorResponse("Invalid User Name.");


        if (!AuthHelper.VerifyPasswordHash(user.PassWordHash, user.PassWordSalt, dto.Password))
        {
            return Utilities.ValidationErrorResponse("Invalid Password");
        }
        var response = new UserDto
        {
            UserName = user.UserName,
            Token = _tokenService.CreateToken(user),
            Picture = user.PicturePath,
        };
        return Utilities.SuccessResponseForGet(response);

    }
    private async Task<bool> UserExists(string userName)
    {
        return await _unitOfWork.Users.Any(user => user.UserName == userName);
    }


}
