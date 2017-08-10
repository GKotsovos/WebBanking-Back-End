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
        TransactionHistoryService transactionHistoryService;

        public CardServices()
        {
            cardManager = new CardManager();
            accountServices = new AccountServices();
            transactionHistoryService = new TransactionHistoryService();
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

        //public List<DebitCard> GetAllCustomerDebitCards(string customerId)
        //{
        //    return cardManager.GetAllCustomerDebitCards(customerId);
        //}

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

        public CreditCard GetCreditCard(string cardId)
        {
            return cardManager.GetCreditCardById(cardId);
        }

        public void CreditCardPayment(string customerId, CreditCardPaymentDetails creditCardPaymentDetails)
        {
            var debitAccount = accountServices.GetAccountByIban(creditCardPaymentDetails.DebitAccount);
            var creditCard = cardManager.GetCreditCardById(creditCardPaymentDetails.CardId);
            decimal totalDebitAmount = creditCardPaymentDetails.Amount + creditCardPaymentDetails.Expenses;

            if (debitAccount.AvailableBalance >= totalDebitAmount)
            {
                if (creditCard.NextInstallmentAmount >= creditCardPaymentDetails.Amount)
                {
                    creditCard.NextInstallmentAmount -= creditCardPaymentDetails.Amount;
                    creditCard.Debt -= creditCardPaymentDetails.Amount;
                }
                else if (creditCard.Debt >= creditCardPaymentDetails.Amount)
                {
                    decimal difference = creditCardPaymentDetails.Amount - creditCard.NextInstallmentAmount;

                    creditCard.NextInstallmentAmount = 0;
                    creditCard.Debt = -difference;
                }
                else
                {
                    return;
                }

                debitAccount.AvailableBalance -= totalDebitAmount;
                debitAccount.LedgerBalance -= totalDebitAmount;
                cardManager.UpdateCreditCard(creditCard);
                accountServices.UpdateAccount(debitAccount);

                var transactionHistory = new TransactionHistory();
                transactionHistory.Amount = creditCardPaymentDetails.Amount;
                transactionHistory.Beneficiary = "Agile Bank";
                transactionHistory.Currency = creditCardPaymentDetails.Currency;
                transactionHistory.CustomerId = customerId;
                transactionHistory.Date = creditCardPaymentDetails.Date;
                transactionHistory.Details = "ΠΛΗΡΩΜΗ ΠΙΣΤΩΤΙΚΗΣ";
                transactionHistory.LedgerBalance = debitAccount.LedgerBalance;
                transactionHistory.ProductId = debitAccount.Iban;
                transactionHistory.TransactionType = "debit";
                transactionHistoryService.AddTransactionHistory(transactionHistory);
            }
        }

        public List<CreditCard> GetAllCustomerCreditCards(string customerId)
        {
            return cardManager.GetAllCustomerCreditCards(customerId);
        }

        public PrepaidCard GetPrePaidCard(string cardId)
        {
            return cardManager.GetPrepaidCardById(cardId);
        }

        public List<PrepaidCard> GetAllCustomerPrepaidCards(string customerId)
        {
            return cardManager.GetAllCustomerPrepaidCards(customerId);
        }
    }
}
