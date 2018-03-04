using System;
using System.Collections.Generic;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class TransactionServices
    {
        TransactionManager transactionManager;

        public TransactionServices()
        {
            transactionManager = new TransactionManager();
        }

        public List<Transaction> GetCurrentMonthProductTransactions(string productId)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1.0);
            var transactionHistory = transactionManager.GetProductTransactionsByTimePeriod(productId, startDate, endDate);
            transactionHistory.Sort((x,y) => x.Date.CompareTo(y.Date));
            transactionHistory.Reverse();
            return transactionHistory;
        }

        public List<Transaction> GetProductTransactionsByTimePeriod(string productId, DateTime startDate, DateTime endDate)
        {
            var transactionHistory = transactionManager.GetProductTransactionsByTimePeriod(productId, startDate, endDate);
            transactionHistory.Sort((x, y) => x.Date.CompareTo(y.Date));
            transactionHistory.Reverse();
            return transactionHistory;
        }

        public IEnumerable<Transaction> GetAllCustomerTransaction(string customerId)
        {
            return transactionManager.GetAllCustomerTransaction(customerId);
        }

        public void LogTransaction(string customerId, string title, decimal newAvailableAmount, TransactionDTO transactionDTO)
        {
            var transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.DebitProductId = transactionDTO.DebitProductId;
            transaction.CreditProductId = transactionDTO.CreditProductId;
            transaction.Amount = transactionDTO.Amount;
            transaction.TransactionType = "debit";
            transaction.Beneficiary = transactionDTO.Beneficiary;
            transaction.Bank = transactionDTO.Bank;
            transaction.Currency = transactionDTO.Currency;
            transaction.Title = title;
            transaction.NewBalance = newAvailableAmount;
            transaction.Date = transactionDTO.IsAsap ? DateTime.Now : transactionDTO.Date;
            transactionManager.AddTransaction(transaction);
        }
    }
}
