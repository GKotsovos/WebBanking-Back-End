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

        public Account GetAccountById(string Id, out TransactionResult transactionResult)
        {
            transactionResult = new TransactionResult(false, "");
            var account = accountMamager.GetAccountById(Id);
            if (account == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Ο λογαριασμός δε βρέθηκε";
            }
            return account;
        }

        public List<Account> GetAllCustomerAccounts(string customerId)
        {
            return accountMamager.GetAllCustomerAccounts(customerId).ToList();
        }

        public TransactionResult UpdateAccount(Account account)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                accountMamager.UpdateAccount(account);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Λάθος κατά την ενημέρωση του λογαριασμού";
            }
            return transactionResult;
        }
    }
}
