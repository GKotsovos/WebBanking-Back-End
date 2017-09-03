using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public partial class Transaction
    {
        public string CustomerId { get; set; }
        public string DebitProductId { get; set; }
        public string TransactionType { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Beneficiary { get; set; }
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }
        public string Currency { get; set; }
        public long Id { get; set; }
        public string CreditProductId { get; set; }
        public string Bank { get; set; }
        public string Comments { get; set; }
    }
}
