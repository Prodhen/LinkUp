using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TodoItemUpdateDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; } = null;
        public DateTime? DueDate { get; set; }
        public bool? IsCompleted { get; set; }

    }
}