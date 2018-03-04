using System;
using System.Collections.Generic;
using System.Linq;
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

        public Account GetAccountById(string Id, out TransactionResult transactionResult, string language)
        {
            transactionResult = new TransactionResult(false, "");
            var account = accountMamager.GetAccountById(Id);
            if (account == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Ο λογαριασμός δε βρέθηκε" : "The account was not found";
            }
            return account;
        }

        public List<Account> GetAllCustomerAccounts(string customerId)
        {
            return accountMamager.GetAllCustomerAccounts(customerId).ToList();
        }

        public TransactionResult UpdateAccount(Account account, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                accountMamager.UpdateAccount(account);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Λάθος κατά την ενημέρωση του λογαριασμού" : "There was a problem while updating the account";
            }
            return transactionResult;
        }
    }
}
