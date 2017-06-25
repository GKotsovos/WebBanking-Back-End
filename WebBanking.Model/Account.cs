using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public class Account
    {
        public string Iban { get; set; }
        public string CustomerId { get; set; }
        public string Currency { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public bool State { get; set; }
        public decimal? Overdraft { get; set; }
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public DateTime LastMovementDate { get; set; }
    }
}
