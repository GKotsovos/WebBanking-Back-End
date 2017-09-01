using System;
using System.Collections.Generic;

namespace WebBanking.Model
{
    public partial class AccountOrder
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string CreditProduct { get; set; }
        public string DebitProduct { get; set; }
        public decimal Amount { get; set; }
        public DateTime NextExecutionDate { get; set; }
        public int ExecutionsLeft { get; set; }
        public string ExecutionFrequency { get; set; }
        public bool State { get; set; }
        public string Comments { get; set; }
        public string Currency { get; set; }
    }
}
