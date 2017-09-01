using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public class OrganizationOrder
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CreditProduct { get; set; }
        public string Organization { get; set; }
        public long OrderCode { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal MaxCreditAmount { get; set; }
        public bool State { get; set; }
        public string Currency { get; set; }
    }
}
