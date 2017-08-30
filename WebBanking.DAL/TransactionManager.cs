using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class TransactionManager
    {
        public List<Transaction> GetTransactionByProductId(string productId)
        {
            using (var bankContext = new BankingContext())
            {
                var transactionHistory = bankContext.Transaction
                    .Where(_transaction => _transaction.DebitProduct == productId);

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
