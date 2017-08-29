using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class CardServices
    {
        CardManager cardManager;

        public CardServices()
        {
            cardManager = new CardManager();
        }

        public void DeleteLinkedProduct(string cardId, string productId)
        {
            cardManager.DeleteLinkedProduct(cardId, productId);
        }

        public CreditCard GetCreditCardById(string cardId)
        {
            return cardManager.GetCreditCardById(cardId);
        }

        public void UpdateCreditCard(CreditCard creditCard)
        {
            cardManager.UpdateCreditCard(creditCard);
        }

        public List<CreditCard> GetAllCustomerCreditCards(string customerId)
        {
            return cardManager.GetAllCustomerCreditCards(customerId);
        }

        public PrepaidCard GetPrePaidCardById(string cardId)
        {
            return cardManager.GetPrepaidCardById(cardId);
        }

        public List<PrepaidCard> GetAllCustomerPrepaidCards(string customerId)
        {
            return cardManager.GetAllCustomerPrepaidCards(customerId);
        }

        public void UpdatePrepaidCard(PrepaidCard prepaidCard)
        {
            cardManager.UpdatePrepaidCard(prepaidCard);
        }

        public TransactionResult PrepaidCardLoadUsingAccount(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitAccount = accountServices.GetAccountByIban(cardTransaction.DebitAccount);
            if (debitAccount != null)
            {
                var prepaidCard = cardManager.GetPrepaidCardById(cardTransaction.CardId);
                if (prepaidCard != null)
                {
                    transactionResult = PrepaidCardLoad(prepaidCard, cardTransaction, debitAccount.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitAccount.AvailableBalance -= totalDebitAmount;
                        debitAccount.LedgerBalance -= totalDebitAmount;
                        accountServices.UpdateAccount(debitAccount);
                        cardManager.UpdatePrepaidCard(prepaidCard);
                        AddPrepaidCardLoadTransactionHistory(customerId, cardTransaction, debitAccount.Iban, debitAccount.LedgerBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Ο λογαριασμός χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult PrepaidCardLoadUsingLoan(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitLoan = loanServices.GetLoanById(cardTransaction.DebitAccount);
            if (debitLoan != null)
            {
                var prepaidCard = cardManager.GetPrepaidCardById(cardTransaction.CardId);
                if (prepaidCard != null)
                {
                    transactionResult = PrepaidCardLoad(prepaidCard, cardTransaction, debitLoan.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitLoan.AvailableBalance -= totalDebitAmount;
                        loanServices.LoanUpdate(debitLoan);
                        cardManager.UpdatePrepaidCard(prepaidCard);
                        AddPrepaidCardLoadTransactionHistory(customerId, cardTransaction, debitLoan.Id, debitLoan.AvailableBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το δάνειο χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult PrepaidCardLoadUsingCreditCard(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitCreditCard = GetCreditCardById(cardTransaction.DebitAccount);
            if (debitCreditCard != null)
            {
                var prepaidCard = cardManager.GetPrepaidCardById(cardTransaction.CardId);
                if (prepaidCard != null)
                {
                    transactionResult = PrepaidCardLoad(prepaidCard, cardTransaction, debitCreditCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitCreditCard.AvailableLimit -= totalDebitAmount;
                        cardManager.UpdateCreditCard(debitCreditCard);
                        cardManager.UpdatePrepaidCard(prepaidCard);
                        AddPrepaidCardLoadTransactionHistory(customerId, cardTransaction, debitCreditCard.Id, debitCreditCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult PrepaidCardLoadUsingPrepaidCard(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitPrepaidCard = GetPrePaidCardById(cardTransaction.DebitAccount);
            if (debitPrepaidCard != null)
            {
                var prepaidCard = cardManager.GetPrepaidCardById(cardTransaction.CardId);
                if (prepaidCard != null)
                {
                    transactionResult = PrepaidCardLoad(prepaidCard, cardTransaction, debitPrepaidCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitPrepaidCard.AvailableLimit -= totalDebitAmount;
                        debitPrepaidCard.LedgerBalance -= totalDebitAmount;
                        cardManager.UpdatePrepaidCard(debitPrepaidCard);
                        cardManager.UpdatePrepaidCard(prepaidCard);
                        AddPrepaidCardLoadTransactionHistory(customerId, cardTransaction, debitPrepaidCard.Id, debitPrepaidCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        private TransactionResult PrepaidCardLoad(PrepaidCard prepaidCard, CardTransaction cardTransaction, decimal debitAccountAvailableBalance)
        {
            var transactionResult = new TransactionResult(false, "");

            if (debitAccountAvailableBalance >= (cardTransaction.Amount + cardTransaction.Expenses))
            {
                prepaidCard.AvailableLimit += cardTransaction.Amount;
                prepaidCard.LedgerBalance += cardTransaction.Amount;
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
            }

            return transactionResult;
        }

        public void AddPrepaidCardLoadTransactionHistory(string customerId, CardTransaction cardTransaction, string debitAccountId, decimal newAvailableAmount)
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
            transactionHistoryService.AddTransaction(transaction);
        }
    }
}
