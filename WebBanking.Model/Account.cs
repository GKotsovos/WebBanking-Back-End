using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public partial class Account : IHasBalances
    {
        public string Id { get; set; }
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
