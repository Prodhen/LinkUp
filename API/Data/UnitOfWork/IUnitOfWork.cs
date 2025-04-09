using System;
using API.Data.Interfaces;

namespace API.Data.UnitOfWork;

public interface IUnitOfWork
{
    int Save();
    Task<int> SaveAsync();
    int LoggedInUserId();
    IUsersRepository Users { get; set; }
    ITodosRepository Todos { get; set; }
}
