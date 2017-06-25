using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public class Loan
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public decimal LoanedAmount { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime RepaymentDate { get; set; }
        public DateTime NextInstallmentDate { get; set; }
        public decimal NextInstallmentAmount { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal Debt { get; set; }
    }
}
