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
        AccountServices accountServices;
        LoanServices loanServices;
        TransactionService transactionHistoryService;

        public CardServices(AccountServices accountServices, LoanServices loanServices, TransactionService transactionHistoryService)
        {
            cardManager = new CardManager();
            this.accountServices = accountServices;
            this.loanServices = loanServices;
            this.transactionHistoryService = transactionHistoryService;
        }

        public DebitCardWithLinkedProducts GetDebitCardWithLinkedProducts(string cardId)
        {
            var debitCard = cardManager.GetDebitCardById(cardId);
            var linkedProducts = cardManager
                .GetDebitCardLinkedProducts(cardId)
                .Select(linkedProduct => accountServices.GetAccountByIban(linkedProduct.ProductId))
                .ToList();
            return new DebitCardWithLinkedProducts(debitCard, linkedProducts);
        }

        public List<DebitCardWithLinkedProducts> GetAllCustomerDebitCardsLinkedProducts(string customerId)
        {
            var debitCards = cardManager.GetAllCustomerDebitCards(customerId);
            var linkedProducts = cardManager.GetAllCustomerDebitCardsLinkedProducts(customerId);

            var cardWithLinkedProducts = new List<DebitCardWithLinkedProducts>();

            foreach (var debitCard in debitCards)
            {
                var linkedAccounts = new List<Account>();

                foreach (var linkedProdct in linkedProducts)
                {
                    if (debitCard.Id == linkedProdct.CardId)
                    {
                        linkedAccounts.Add(accountServices.GetAccountByIban(linkedProdct.ProductId));
                    }
                }
                cardWithLinkedProducts.Add(new DebitCardWithLinkedProducts(debitCard, linkedAccounts));
            }

            return cardWithLinkedProducts;
        }

        public void DeleteLinkedProduct(string cardId, string productId)
        {
            cardManager.DeleteLinkedProduct(cardId, productId);
        }

        public CreditCard GetCreditCardById(string cardId)
        {
            return cardManager.GetCreditCardById(cardId);
        }

        public TransactionResult CreditCardPaymentUsingAccount(string customerId, CreditCardPaymentDetails creditCardPaymentDetails)
        public TransactionResult CreditCardPaymentUsingAccount(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitAccount = accountServices.GetAccountByIban(cardTransaction.DebitAccount);
            if (debitAccount != null)
            {
                var creditCard = GetCreditCardById(cardTransaction.CardId);
                if (creditCard != null)
                {
                    transactionResult = CreditCardPayment(creditCard, cardTransaction, debitAccount.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitAccount.AvailableBalance -= totalDebitAmount;
                        debitAccount.LedgerBalance -= totalDebitAmount;
                        accountServices.UpdateAccount(debitAccount);
                        cardManager.UpdateCreditCard(creditCard);
                        AddCreditCardPaymentTransactionHistory(customerId, creditCardPaymentDetails, debitAccount.Iban, debitAccount.LedgerBalance);
                        AddCreditCardPaymentTransactionHistory(customerId, cardTransaction, debitAccount.Iban, debitAccount.LedgerBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα δεν βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Ο λογαριασμός χρέωσης δεν βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult CreditCardPaymentUsingLoan(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitLoan = loanServices.GetLoanById(cardTransaction.DebitAccount);
            if (debitLoan != null)
            {
                var creditCard = GetCreditCardById(cardTransaction.CardId);
                if (creditCard != null)
                {
                    transactionResult = CreditCardPayment(creditCard, cardTransaction, debitLoan.AvailableBalance);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitLoan.AvailableBalance -= totalDebitAmount;
                        loanServices.Update(debitLoan);
                        cardManager.UpdateCreditCard(creditCard);
                        AddCreditCardPaymentTransactionHistory(customerId, creditCardPaymentDetails, debitLoan.Id, debitLoan.AvailableBalance);
                        AddCreditCardPaymentTransactionHistory(customerId, cardTransaction, debitLoan.Id, debitLoan.AvailableBalance);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα δεν βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το δάνειο χρέωσης δεν βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult CreditCardPaymentUsingCreditCard(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitCreditCard = GetCreditCardById(cardTransaction.DebitAccount);
            if (debitCreditCard != null)
            {
                var creditCard = GetCreditCardById(cardTransaction.CardId);
                if (creditCard != null)
                {
                    transactionResult = CreditCardPayment(creditCard, cardTransaction, debitCreditCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitCreditCard.AvailableLimit -= totalDebitAmount;
                        cardManager.UpdateCreditCard(debitCreditCard);
                        cardManager.UpdateCreditCard(creditCard);
                        AddCreditCardPaymentTransactionHistory(customerId, creditCardPaymentDetails, debitCreditCard.Id, debitCreditCard.AvailableLimit);
                        AddCreditCardPaymentTransactionHistory(customerId, cardTransaction, debitCreditCard.Id, debitCreditCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα δεν βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα χρέωσης δεν βρέθηκε");
            }

            return transactionResult;
        }

        public TransactionResult CreditCardPaymentUsingPrepaidCard(string customerId, CardTransaction cardTransaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            var debitPrepaidCard = GetPrePaidCardById(cardTransaction.DebitAccount);
            if (debitPrepaidCard != null)
            {
                var creditCard = GetCreditCardById(cardTransaction.CardId);
                if (creditCard != null)
                {
                    transactionResult = CreditCardPayment(creditCard, cardTransaction, debitPrepaidCard.AvailableLimit);
                    if (!transactionResult.HasError)
                    {
                        decimal totalDebitAmount = cardTransaction.Amount + cardTransaction.Expenses;
                        debitPrepaidCard.AvailableLimit -= totalDebitAmount;
                        debitPrepaidCard.LedgerBalance -= totalDebitAmount;
                        cardManager.UpdatePrepaidCard(debitPrepaidCard);
                        cardManager.UpdateCreditCard(creditCard);
                        AddCreditCardPaymentTransactionHistory(customerId, creditCardPaymentDetails, debitPrepaidCard.Id, debitPrepaidCard.AvailableLimit);
                        AddCreditCardPaymentTransactionHistory(customerId, cardTransaction, debitPrepaidCard.Id, debitPrepaidCard.AvailableLimit);
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η πιστωτική κάρτα δεν βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα χρέωσης δεν βρέθηκε");
            }

            return transactionResult;
        }

        private TransactionResult CreditCardPayment(CreditCard creditCard, CardTransaction cardTransaction, decimal debitAccountAvailableBalance)
        {
            var transactionResult = new TransactionResult(false, "");

            if (debitAccountAvailableBalance >= (cardTransaction.Amount + cardTransaction.Expenses))
            {
                if ((creditCard.NextInstallmentAmount < cardTransaction.Amount) && (creditCard.Debt < cardTransaction.Amount))
                {
                    transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από το σύνολο οφειλών");
                }
                else if (creditCard.Debt < cardTransaction.Amount)
                {
                    transactionResult = new TransactionResult(true, "Το ποσό πληρωμής είναι μεγαλύτερο από την τρέχων οφειλή");
                }
                else if (creditCard.NextInstallmentAmount >= cardTransaction.Amount)
                {
                    creditCard.NextInstallmentAmount -= cardTransaction.Amount;
                    creditCard.Debt -= cardTransaction.Amount;
                }
                else
                {
                    creditCard.NextInstallmentAmount = 0;
                    creditCard.Debt -= cardTransaction.Amount;
                }                             
            }
            else
            {
                transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
            }

            return transactionResult;
        }
        
        public void AddCreditCardPaymentTransactionHistory(string customerId, CardTransaction cardTransaction, string debitAccountId, decimal newAvailableAmount)
        {
            var transaction = new Transaction();
            transaction.Amount = cardTransaction.Amount;
            transaction.Beneficiary = "Agile Bank";
            transaction.Currency = cardTransaction.Currency;
            transaction.CustomerId = customerId;
            transaction.Date = cardTransaction.Date;
            transaction.Details = "ΠΛΗΡΩΜΗ ΠΙΣΤΩΤΙΚΗΣ";
            transaction.LedgerBalance = newAvailableAmount;
            transaction.ProductId = debitAccountId;
            transaction.TransactionType = "debit";
            transactionHistoryService.AddTransaction(transaction);
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
    }
}
