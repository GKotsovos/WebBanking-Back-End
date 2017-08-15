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
    }
}
