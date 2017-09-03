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
        TransactionServices transactionServices;
        Helper helper;

        public TransferServices(AccountServices accountServices, CardServices cardServices, LoanServices loanServices, TransactionServices transactionServices)
        {
            this.accountServices = accountServices;
            this.transactionServices = transactionServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public TransactionResult CheckDebitBalance(IHasBalances debitProduct, decimal debitAmount)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (debitProduct.AvailableBalance <= 0 || debitProduct.AvailableBalance < debitAmount)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Λάθος υπόλοιπο λογαριασμού";
            }
            return transactionResult;
        }

        public TransactionResult Transfer(string customerId, TransactionDTO transaction)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances debitProduct = helper.GetProduct(transaction.DebitProductType, transaction.DebitProductId, out transactionResult);
            transactionResult = CheckDebitBalance(debitProduct, transaction.Amount);
            if (!transactionResult.HasError)
            {
                DebitProduct(debitProduct, transaction.Amount, transaction.Expenses);
                transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct);
                if (!transactionResult.HasError && transaction.Bank == "AGILGRAA")
                {
                    Account creditProduct = accountServices.GetAccountById(transaction.CreditProductId, out transactionResult);
                    if (!transactionResult.HasError)
                    {
                        CreditProduct(creditProduct, transaction.Amount);
                        transactionResult = accountServices.UpdateAccount(creditProduct);
                    }
                }
                if (!transactionResult.HasError)
                {
                    transactionServices.LogTransaction(customerId, "ΜΕΤΑΦΟΡΑ ΧΡΗΜΑΤΩΝ", debitProduct.AvailableBalance, transaction);
                }
            }

            return transactionResult;
        }

        public void DebitProduct(IHasBalances debitProduct, decimal amount, decimal expenses)
        {
            debitProduct.AvailableBalance -= amount + expenses;
            debitProduct.LedgerBalance -= amount + expenses;
        }

        public void CreditProduct(IHasBalances creditProduct, decimal amount)
        {
            creditProduct.AvailableBalance += amount;
            creditProduct.LedgerBalance += amount;
        }
    }
}
