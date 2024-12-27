using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class gameRequestDto
    {
        [Required]
        public string Name {get; set;}
        [Required]
        public string Description {get; set;}
        [Required]
        public string Type {get; set;}

        [Required]
        public int Target { get; set; }

        [Required]
        public decimal Profit { get; set; }

        [Required]
        public decimal Percentage{ get; set; }
    }
}
