using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using usersAuthApi.Models.Domain;

namespace usersAuthApi.Models.DTO
{
    public class FlappyBirdGameTimeRequestDto
    {
        [Required]
        public int PId { get; set; }
        [Required]
        public int GId { get; set; }
        [Required]
        public decimal BetAmount { get; set; }
        [Required]
        public TimeOnly StartGameTime { get; set; }
        [Required]
        public TimeOnly EndGameTime { get; set; }
        
    }
}
