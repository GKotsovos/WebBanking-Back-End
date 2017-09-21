using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public partial class PaymentOrder
    {
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public string DebitProductId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal MaxPaymentAmount { get; set; }
        public bool State { get; set; }
        public string Currency { get; set; }
        public DateTime PreviousExecutionDate { get; set; }
        public decimal Charges { get; set; }
    }
}
