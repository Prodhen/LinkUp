using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TodoItemAddDto
    {
        public required string Title { get; set; }
        public string? Description { get; set; } = null;
        public DateTime? DueDate { get; set; }
        public int UserId { get; set; }

    }
}