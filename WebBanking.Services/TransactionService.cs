using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class TransactionService
    {
        TransactionManager transactionManager;

        public TransactionService()
        {
            transactionManager = new TransactionManager();
        }

        public List<Transaction> GetProductTransaction(string productId)
        {
            var transactionHistory = transactionManager.GetTransactionByProductId(productId);
            transactionHistory.Reverse();
            return transactionHistory;
        }

        public IEnumerable<Transaction> GetAllCustomerTransaction(string customerId)
        {
            return transactionManager.GetAllCustomerTransaction(customerId);
        }

        public void AddTransaction(Transaction transactionHistory)
        {
            transactionManager.AddTransaction(transactionHistory);
        }

        public void AddCreditCardPaymentTransaction(string customerId, CardTransaction cardTransaction, string debitAccountId, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.Amount = cardTransaction.Amount;
            transaction.Beneficiary = "Agile Bank";
            transaction.Currency = cardTransaction.Currency;
            transaction.CustomerId = customerId;
            transaction.Date = cardTransaction.Date;
            transaction.Details = "ΠΛΗΡΩΜΗ ΠΙΣΤΩΤΙΚΗΣ";
            transaction.LedgerBalance = newAvailableAmount;
            transaction.ProductId = debitAccountId;
            transaction.TransactionType = "debit";
            AddTransaction(transaction);
        }

        public void AddPrepaidCardLoadTransaction(string customerId, CardTransaction cardTransaction, string debitAccountId, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.Amount = cardTransaction.Amount;
            transaction.Beneficiary = "Agile Bank";
            transaction.Currency = cardTransaction.Currency;
            transaction.CustomerId = customerId;
            transaction.Date = cardTransaction.Date;
            transaction.Details = "ΦΟΡΤΙΣΗ ΠΡΟΠΛΗΡΩΜΕΝΗΣ";
            transaction.LedgerBalance = newAvailableAmount;
            transaction.ProductId = debitAccountId;
            transaction.TransactionType = "debit";
            AddTransaction(transaction);
        }

        public void AddLoanPaymentTransactionHistory(string customerId, LoanTransaction loanTransaction, string debitAccountId, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.Amount = loanTransaction.Amount;
            transaction.Beneficiary = "Agile Bank";
            transaction.Currency = loanTransaction.Currency;
            transaction.CustomerId = customerId;
            transaction.Date = loanTransaction.Date;
            transaction.Details = "ΠΛΗΡΩΜΗ ΔΑΝΕΙΟΥ";
            transaction.LedgerBalance = newAvailableAmount;
            transaction.ProductId = debitAccountId;
            transaction.TransactionType = "debit";
            AddTransaction(transaction);
        }
    }
}
