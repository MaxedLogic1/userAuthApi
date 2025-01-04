using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class loginResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        //public bool IsActive { get; set; }
        //public string RandomId { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
