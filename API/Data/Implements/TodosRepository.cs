using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interfaces;
using API.Entities;

namespace API.Data.Implements
{
    public class TodosRepository : GenericRepository<TodoItem>, ITodosRepository
    {

        public TodosRepository(DataContext _context) : base(_context) { }

    }
}