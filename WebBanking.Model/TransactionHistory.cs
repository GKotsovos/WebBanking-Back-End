using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public partial class TransactionHistory
    {
        public string CustomerId { get; set; }
        public string ProductId { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Details { get; set; }
        public string Beneficiary { get; set; }
        public decimal Amount { get; set; }
        public decimal LedgerBalance { get; set; }
        public string Currency { get; set; }
        public int Id { get; set; }
    }
}
