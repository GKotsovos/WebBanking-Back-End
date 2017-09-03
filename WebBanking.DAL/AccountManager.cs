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
                return bankContext.Account
                    .FirstOrDefault(_account => _account.Id == Id);
            }
        }

        public List<Account> GetAllCustomerAccounts(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var accounts = bankContext.Account
                    .Where(_account => _account.CustomerId == customerId);

                if (accounts != null)
                {
                    return accounts.ToList();
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
