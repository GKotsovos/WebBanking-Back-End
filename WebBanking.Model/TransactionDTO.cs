using System;

namespace WebBanking.Model
{
    public class TransactionDTO
    {
        public string DebitProductId { get; set; }
        public string DebitProductType { get; set; }
        public string CreditProductId { get; set; }
        public string CreditProductType { get; set; }
        public string Beneficiary { get; set; }
        public string Bank { get; set; }
        public bool IsTransfer { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public bool IsAsap { get; set; }
        public decimal Expenses { get; set; }
        public string Comments { get; set; }
    }
}
