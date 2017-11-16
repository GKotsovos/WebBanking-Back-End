using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class TransactionManager
    {
        public List<Transaction> GetProductTransactionsByTimePeriod(string productId, DateTime startDate, DateTime endDate)
        {
            using (var bankContext = new BankingContext())
            {
                var transactionHistory = bankContext.Transaction
                    .Where(transaction => transaction.DebitProductId == productId && 
                        transaction.Date >= startDate && 
                        transaction.Date <= endDate);

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

        public List<Transaction> GetAllCustomerTransaction(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var transactionHistorys = bankContext.Transaction
                    .Where(transactionHistory => transactionHistory.CustomerId == customerId);

                if (transactionHistorys != null)
                {
                    return transactionHistorys.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.Transaction.Add(transaction);
                bankContext.SaveChanges();
            }
        }
    }
}
