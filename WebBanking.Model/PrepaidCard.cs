﻿using System;

namespace WebBanking.Model
{
    public partial class PrepaidCard : IHasBalances
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Brand { get; set; }
        public string Currency { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal LedgerBalance { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Status { get; set; }
    }
}
