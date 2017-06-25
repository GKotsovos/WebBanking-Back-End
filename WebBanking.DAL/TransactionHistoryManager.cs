using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class TransactionHistoryManager
    {
        public List<TransactionHistory> GetTransactionHistoryByProductId(string productId)
        {
            using (var bankContext = new BankingContext())
            {
                var transactionHistory = bankContext.TransactionHistory
                    .Where(_transactionHistory => _transactionHistory.ProductId == productId);

                if (transactionHistory != null)
                {
                    return transactionHistory.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<TransactionHistory> GetAllCustomerTransactionHistory(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var transactionHistorys = bankContext.TransactionHistory
                    .Where(transactionHistory => transactionHistory.CustomerId == customerId)
                    .ToList();

                if (transactionHistorys != null)
                {
                    return transactionHistorys;
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddTransactionHistory(TransactionHistory transactionHistory)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.TransactionHistory.Add(transactionHistory);
                bankContext.SaveChanges();
            }
        }
    }
}
