using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class forgetPasswordDto
    {
        [Required]
        public string UserName { get; set; }
    }
}
