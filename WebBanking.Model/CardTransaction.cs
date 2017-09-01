using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class CardTransaction
    {
        public string CardId { get; set; }
        public string DebitProduct { get; set; }
        public string DebitProductType { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public decimal Expenses { get; set; }
        public DateTime Date { get; set; }
    }
}
