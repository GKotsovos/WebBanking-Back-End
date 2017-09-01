using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class AccountManager
    {
        public Account GetAccountById(string Id)
        {
            using (var bankContext = new BankingContext())
            {
                var account = bankContext.Account
                    .FirstOrDefault(_account => _account.Id == Id);

                if (account != null)
                {
                    return account;
                }
                else
                {
                    return null;
                }
            }
        }

        public List<Account> GetAllCustomerAccounts(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var accounts = bankContext.Account
                    .Where(_account => _account.CustomerId == customerId)
                    .ToList();

                if (accounts != null)
                {
                    return accounts;
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdateAccount(Account account)
        {
            using (var bankContext = new BankingContext())
            {
                try
                {
                    bankContext.Account.Update(account as Account);
                    bankContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
