using System;
using System.Linq.Expressions;
using System.Security.Claims;
using API.Data.Implements;
using API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _dataContext;
    private readonly IHttpContextAccessor _httpContext;

    public UnitOfWork(DataContext dataContext, IHttpContextAccessor httpContext)
    {
        this._dataContext = dataContext;
        this._httpContext = httpContext;
        Users = new UsersRepository(_dataContext);
        Todos = new TodosRepository(_dataContext);

    }
    public IUsersRepository Users { get; set; }
    public ITodosRepository Todos { get; set; }

    public int LoggedInUserId()
    {
        var loggedInUserId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(loggedInUserId)) throw new UnauthorizedAccessException();
        return Convert.ToInt32(loggedInUserId);
    }



    public int Save()
    {
        return _dataContext.SaveChanges();
    }

    public Task<int> SaveAsync()
    {
        return _dataContext.SaveChangesAsync();
    }


}
