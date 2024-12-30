using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class HeadTailGameMultiUsersRequestDto
    {
        [Required]
        public List<HeadTailGameReuestDto>? Bets  { get; set; }

    }
}
