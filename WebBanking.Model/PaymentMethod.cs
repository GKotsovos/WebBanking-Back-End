﻿namespace WebBanking.Model
{
    public partial class PaymentMethod
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Name { get; set; }
        public decimal Charges { get; set; }
    }
}
