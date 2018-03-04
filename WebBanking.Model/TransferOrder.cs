using System;

namespace WebBanking.Model
{
    public partial class TransferOrder
    {
        public long Id { get; set; }
        public string CustomerId { get; set; }
        public string CreditProductId { get; set; }
        public string DebitProductId { get; set; }
        public decimal Amount { get; set; }
        public DateTime NextExecutionDate { get; set; }
        public int ExecutionsLeft { get; set; }
        public int ExecutionFrequency { get; set; }
        public bool State { get; set; }
        public string Comments { get; set; }
        public string Currency { get; set; }
        public string CustomTitle { get; set; }
        public decimal Charges { get; set; }
    }
}
