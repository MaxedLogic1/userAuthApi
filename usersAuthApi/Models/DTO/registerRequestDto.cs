using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class registerRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
