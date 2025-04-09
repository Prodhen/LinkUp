using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TodoItem
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; } = null;
        public bool? IsCompleted { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        public virtual  AppUser AppUser { get; set; }

    }
}