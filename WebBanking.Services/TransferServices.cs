using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.Services.HelperMethods;

namespace WebBanking.Services
{
    public class TransferServices
    {
        AccountServices accountServices;
        Helper helper;

        public TransferServices(AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            this.accountServices = accountServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public TransactionResult CheckDebitBalance(IHasBalances DebitProduct, decimal debitAmount)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (DebitProduct.AvailableBalance <= 0 || DebitProduct.AvailableBalance < debitAmount)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Λάθος υπόλοιπο λογαριασμού";
            }
            return transactionResult;
        }

        public TransactionResult Transfer(TransactionDTO transaction)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances DebitProduct = helper.GetProduct(transaction.DebitProductIdType, transaction.DebitProductId, out transactionResult);
            transactionResult = CheckDebitBalance(DebitProduct, transaction.Amount);
            if (!transactionResult.HasError)
            {
                DebitProduct(DebitProduct, transaction.Amount, transaction.Expenses);
                transactionResult = helper.UpdateProduct(transaction.DebitProductIdType, DebitProduct);
                if (!transactionResult.HasError && transaction.Bank == "AGILE BANK")
                {
                    Account CreditProduct = accountServices.GetAccountById(transaction.DebitProductId, out transactionResult);
                    if (!transactionResult.HasError)
                    {
                        CreditProduct(CreditProduct, transaction.Amount);
                        transactionResult = accountServices.UpdateAccount(CreditProduct);
                    }
                }
            }

            return transactionResult;
        }

        public void DebitProduct(IHasBalances DebitProduct, decimal amount, decimal expenses)
        {
            DebitProduct.AvailableBalance -= amount + expenses;
            DebitProduct.LedgerBalance -= amount + expenses;
        }

        public void CreditProduct(IHasBalances CreditProduct, decimal amount)
        {
            CreditProduct.AvailableBalance += amount;
            CreditProduct.LedgerBalance += amount;
        }
    }
}
