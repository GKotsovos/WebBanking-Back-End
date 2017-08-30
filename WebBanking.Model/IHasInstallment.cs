using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public interface IHasInstallment
    {
        decimal NextInstallmentAmount { get; set; }
        decimal Debt { get; set; }
        DateTime NextInstallmentDate { get; set; }
    }
}
