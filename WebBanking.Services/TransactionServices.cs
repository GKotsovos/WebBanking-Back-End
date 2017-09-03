using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public List<Transaction> GetProductTransaction(string productId)
        {
            var transactionHistory = transactionManager.GetTransactionByProductId(productId);
            transactionHistory.Sort((x,y) => x.Date.CompareTo(y.Date));
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
            transaction.Date = transactionDTO.Date;
            transactionManager.AddTransaction(transaction);
        }
    }
}
