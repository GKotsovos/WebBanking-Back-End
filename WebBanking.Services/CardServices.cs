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

        public CreditCard GetCreditCardById(string cardId, out TransactionResult transactionResult, string language)
        {
            transactionResult = new TransactionResult(false, "");
            var creditCard =  cardManager.GetCreditCardById(cardId);
            if (creditCard == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Η πιστωτική κάρτα δε βρέθηκε" : "The credit card was not found";
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

        public PrepaidCard GetPrePaidCardById(string cardId, out TransactionResult transactionResult, string language)
        {
            transactionResult = new TransactionResult(false, "");
            var prepaidCard =  cardManager.GetPrepaidCardById(cardId);
            if (prepaidCard == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Η προπληρωμένη κάρτα δε βρέθηκε" : "The prepaid card was not found";
            }
            return prepaidCard;
        }

        public List<PrepaidCard> GetAllCustomerPrepaidCards(string customerId)
        {
            return cardManager.GetAllCustomerPrepaidCards(customerId);
        }

        public TransactionResult UpdatePrepaidCard(PrepaidCard prepaidCard, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                cardManager.UpdatePrepaidCard(prepaidCard);
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά την ενημέρωση της προπληρωμένης" : "There was a problem while updating the prepaid card";
            }
            return transactionResult;
        }
    }
}
