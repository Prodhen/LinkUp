using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AddMessageDto
    {
        public required string RecipientUsername { get; set; }
        public required string Content { get; set; }
    }
}