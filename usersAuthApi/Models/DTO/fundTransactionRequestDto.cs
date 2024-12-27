using System.ComponentModel.DataAnnotations;

namespace usersAuthApi.Models.DTO
{
    public class fundTransactionRequestDto
    {
        [Required]
        public int PId { get; set; }
        [Required]
        public decimal CreditAmount { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public IFormFile? Image {  get; set; }  

    }
}
