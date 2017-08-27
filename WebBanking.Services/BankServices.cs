using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class BankServices
    {
        BankManager bankManager;

        public BankServices()
        {
            bankManager = new BankManager();
        }

        public List<string> GetAllDomesticBanks()
        {
            return bankManager.GetAllDomesticBanks()
                .Select(bank => bank.Bic + " - " + bank.Name)
                .ToList();
        }
    }
}
