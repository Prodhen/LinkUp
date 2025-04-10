using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserAddDto
    {
        [Required]
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
