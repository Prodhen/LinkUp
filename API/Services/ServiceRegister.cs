using System;
using API.Data.UnitOfWork;
using API.Services.Implements;
using API.Services.Interface;

namespace API.Services;

public static class ServiceRegister
{
    public static void Register(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        service.AddHttpContextAccessor();
        service.AddScoped<ITodosService, TodosService>();
        service.AddScoped<IUsersService, UserService>();

    }

}
