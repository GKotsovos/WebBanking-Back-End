using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class TransferServices
    {
        public TransactionResult Transfer(IHasBalances debitAccount, Account creditAccount, decimal amount, decimal expenses)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");

            if (debitAccount.AvailableBalance >= amount)
            {
                debitAccount.AvailableBalance -= amount + expenses;
                debitAccount.LedgerBalance -= amount + expenses;
                creditAccount.AvailableBalance += amount;
                creditAccount.LedgerBalance += amount;
            }
            else
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό";
            }

            return transactionResult;
        }
    }
}
