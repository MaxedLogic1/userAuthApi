using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.Domain
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string RandomId { get; set; }

        public DateTime EntryDate { get; set; }
       

    }
}
