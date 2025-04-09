using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data.Interfaces
{
    public interface ITodosRepository : IGenericRepository<TodoItem>
    {

    }
}