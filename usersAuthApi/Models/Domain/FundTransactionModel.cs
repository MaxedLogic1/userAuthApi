﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity;

namespace usersAuthApi.Models.Domain
{
    public class FundTransactionModel
    {
        [Key]
        public int Id { get; set; }
        
        [Column(TypeName = "money")]
        public decimal CreditAmount { get; set; }
        [Column(TypeName = "money")]
        public decimal DebitAmount { get; set; } = 0;
        public DateTime TransactionDate { get; set; }
        public string Remark { get; set; }
        public string Type { get; set; }
        public string TxNoId { get; set; }
        public string? Images { get; set; } 

        public int PId { get; set; }

        [ForeignKey("PId")]
        public UserModel User { get; set; }
    }
}