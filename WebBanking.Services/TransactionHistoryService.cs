﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class TransactionHistoryService
    {
        TransactionHistoryManager transactionHistoryManager;

        public TransactionHistoryService()
        {
            transactionHistoryManager = new TransactionHistoryManager();
        }

        public List<TransactionHistory> GetProductTransactionHistory(string productId)
        {
            var transactionHistory = transactionHistoryManager.GetTransactionHistoryByProductId(productId);
            transactionHistory.Reverse();
            return transactionHistory;
        }

        public IEnumerable<TransactionHistory> GetAllCustomerTransactionHistory(string customerId)
        {
            return transactionHistoryManager.GetAllCustomerTransactionHistory(customerId);
        }

        public void AddTransactionHistory(TransactionHistory transactionHistory)
        {
            transactionHistoryManager.AddTransactionHistory(transactionHistory);
        }
    }
}
