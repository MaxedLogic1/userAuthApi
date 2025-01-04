namespace usersAuthApi.Models.DTO
{
    public class fundTransectionResponseDto
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public  decimal CreditAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Remark { get; set; }
         public DateTime TransactionDate { get; set; }
        public string Type { get; set; }
        //public string TxNoId { get; set; }
        //public string? Image { get; set; }

    }
}
