using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required byte[] PassWordHash { get; set; }
        public required byte[] PassWordSalt { get; set; }
        public string? PicturePath { get; set; }
    }
}