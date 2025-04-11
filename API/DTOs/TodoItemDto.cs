using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class TodoItemDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; } = null;
        public bool? IsCompleted { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}