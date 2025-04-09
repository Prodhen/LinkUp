using System;
using API.Data.UnitOfWork;
using API.Services.Interface;

namespace API.Services.Implements;

public class UserService : IUsersService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork){
        _unitOfWork = unitOfWork;
    }

}
