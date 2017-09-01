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

        public CreditCard GetCreditCardById(string cardId, out TransactionResult transactionResult)
        {
            transactionResult = new TransactionResult(false, "");
            var creditCard =  cardManager.GetCreditCardById(cardId);
            if (creditCard == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Η πιστωτική κάρτα δε βρέθηκε";
            }
            return creditCard;
        }

        public void UpdateCreditCard(CreditCard creditCard)
        {
            cardManager.UpdateCreditCard(creditCard);
        }

        public List<CreditCard> GetAllCustomerCreditCards(string customerId)
        {
            return cardManager.GetAllCustomerCreditCards(customerId);
        }

        public PrepaidCard GetPrePaidCardById(string cardId, out TransactionResult transactionResult)
        {
            transactionResult = new TransactionResult(false, "");
            var prepaidCard =  cardManager.GetPrepaidCardById(cardId);
            if (prepaidCard == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = "Η προπληρωμένη κάρτα δε βρέθηκε";
            }
            return prepaidCard;
        }

        public List<PrepaidCard> GetAllCustomerPrepaidCards(string customerId)
        {
            return cardManager.GetAllCustomerPrepaidCards(customerId);
        }

        public void UpdatePrepaidCard(PrepaidCard prepaidCard)
        {
            cardManager.UpdatePrepaidCard(prepaidCard);
        }
    }
}
