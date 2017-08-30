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
