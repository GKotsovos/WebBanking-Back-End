using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public interface IHasBalances
    {
        string Id { get; set; }
        decimal AvailableBalance { get; set; }
        decimal LedgerBalance { get; set; }
    }
}
