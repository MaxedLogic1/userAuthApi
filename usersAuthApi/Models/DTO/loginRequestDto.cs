using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class loginRequestDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
