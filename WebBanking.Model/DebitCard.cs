using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public class DebitCard
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Brand { get; set; }
        public string Currency { get; set; }
        public decimal DailyLimit { get; set; }
        public decimal AvailableLimit { get; set; }
        public decimal LedgerBalance { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Status { get; set; }
    }
}
