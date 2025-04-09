using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Interfaces;
using API.Entities;

namespace API.Data.Implements
{
    public class UsersRepository : GenericRepository<AppUser>, IUsersRepository
    {
        public UsersRepository(DataContext _context) : base(_context) { }

    }

}