using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class AccountServices
    {
        AccountManager accountMamager;

        public AccountServices()
        {
            accountMamager = new AccountManager();
        }

        public Account GetAccountByIban(string Iban)
        {
            return accountMamager.GetAccountById(Iban);
        }

        public List<Account> GetAllCustomerAccounts(string customerId)
        {
            return accountMamager.GetAllCustomerAccounts(customerId).ToList();
        }

        public void UpdateAccount(Account account)
        {
            accountMamager.UpdateAccount(account);
        }
    }
}
