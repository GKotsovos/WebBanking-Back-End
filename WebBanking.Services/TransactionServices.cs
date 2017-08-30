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

        public void AddCreditCardPaymentTransaction(string customerId, CardTransaction cardTransaction, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.DebitProduct = cardTransaction.DebitAccount;
            transaction.CreditProduct = "AGILGRAA - AGILE BANK";
            transaction.Amount = cardTransaction.Amount;
            transaction.TransactionType = "debit";
            transaction.Beneficiary = "AGILGRAA - AGILE BANK";
            transaction.Bank = "AGILGRAA - AGILE BANK";
            transaction.Currency = cardTransaction.Currency;
            transaction.Title = "ΠΛΗΡΩΜΗ ΠΙΣΤΩΤΙΚΗΣ";
            transaction.NewBalance = newAvailableAmount;
            transaction.Date = cardTransaction.Date;
            AddTransaction(transaction);
        }

        public void AddPrepaidCardLoadTransaction(string customerId, CardTransaction cardTransaction, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.DebitProduct = cardTransaction.DebitAccount;
            transaction.CreditProduct = "AGILGRAA - AGILE BANK";
            transaction.Amount = cardTransaction.Amount;
            transaction.TransactionType = "debit";
            transaction.Beneficiary = "AGILE BANK";
            transaction.Bank = "AGILGRAA - AGILE BANK";
            transaction.Currency = cardTransaction.Currency;
            transaction.Title = "ΦΟΡΤΙΣΗ ΠΡΟΠΛΗΡΩΜΕΝΗΣ";
            transaction.NewBalance = newAvailableAmount;
            transaction.Date = cardTransaction.Date;
            AddTransaction(transaction);
        }

        public void AddLoanPaymentTransaction(string customerId, LoanTransaction loanTransaction, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.DebitProduct = loanTransaction.DebitAccount;
            transaction.CreditProduct = "AGILGRAA - AGILE BANK";
            transaction.Amount = loanTransaction.Amount;
            transaction.TransactionType = "debit";
            transaction.Beneficiary = "AGILE BANK";
            transaction.Bank = "AGILGRAA - AGILE BANK";
            transaction.Currency = loanTransaction.Currency;
            transaction.Title = "ΠΛΗΡΩΜΗ ΔΑΝΕΙΟΥ";
            transaction.NewBalance = newAvailableAmount;
            transaction.Date = loanTransaction.Date;
            AddTransaction(transaction);
        }

        public void AddTransferTransaction(string customerId, TransferTransaction transferTransaction, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.CustomerId = customerId;
            transaction.DebitProduct = transferTransaction.DebitAccount;
            transaction.CreditProduct = transferTransaction.CreditAccount;
            transaction.Amount = transferTransaction.Amount;
            transaction.TransactionType = "debit";
            transaction.Beneficiary = transferTransaction.Beneficiary;
            transaction.Bank = transferTransaction.Bank;
            transaction.Currency = transferTransaction.Currency;
            transaction.Title = "ΜΕΤΑΦΟΡΑ ΧΡΗΜΑΤΩΝ";
            transaction.NewBalance = newAvailableAmount;
            transaction.Date = transferTransaction.Date;
            AddTransaction(transaction);
        }
    }
}
