using System;

namespace WebBanking.Model
{
    public interface IHasInstallment
    {
        decimal NextInstallmentAmount { get; set; }
        decimal Debt { get; set; }
        DateTime NextInstallmentDate { get; set; }
    }
}
