using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class BankManager
    {
        public List<Bank> GetAllDomesticBanks()
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.Bank.Select(bank => bank).ToList();
            }
        }
    }
}
