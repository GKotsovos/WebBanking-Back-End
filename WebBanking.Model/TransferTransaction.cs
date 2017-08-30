using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class TransferTransaction
    {
        public string DebitAccount { get; set; }
        public string DebitAccountType { get; set; }
        public string CreditAccount { get; set; }
        public string Beneficiary { get; set; }
        public string Bank { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public decimal Expenses { get; set; }
        public string Comments { get; set; }
    }
}
