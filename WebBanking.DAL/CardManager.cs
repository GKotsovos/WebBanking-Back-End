using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class CardManager
    {
        public DebitCard GetDebitCardById(string cardId)
        {
            using (var bankContext = new BankingContext())
            {
                return  bankContext.DebitCard
                    .FirstOrDefault(_card => _card.Id == cardId);
            }
        }

        public List<LinkedProducts> GetDebitCardLinkedProducts(string cardId)
        {
            using (var bankContext = new BankingContext())
            {
                var debitCardsWithLinkedProducts = bankContext.LinkedProducts
                    .Where(linkedProduct => linkedProduct.CardId == cardId);
                if (debitCardsWithLinkedProducts != null)
                {
                    return debitCardsWithLinkedProducts.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public List<DebitCard> GetAllCustomerDebitCards(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var debitCards =  bankContext.DebitCard
                    .Where(card => card.CustomerId == customerId);
                if (debitCards != null)
                {
                    return debitCards.ToList();
                }
                else
                {
                    return null;
                }
            }
        }
        

        public List<LinkedProducts> GetAllCustomerDebitCardsLinkedProducts(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var debitCardsWithLinkedProducts = bankContext.LinkedProducts
                    .Where(linkedProduct => linkedProduct.CustomerId == customerId);
                if (debitCardsWithLinkedProducts != null)
                {
                    return debitCardsWithLinkedProducts.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public void DeleteLinkedProduct(string cardId, string productId)
        {
            using (var bankContext = new BankingContext())
            {
                var linkedProduct = bankContext.LinkedProducts
                    .First(product => product.CardId == cardId && product.ProductId == productId);
                if (linkedProduct != null)
                {
                    bankContext.Remove(linkedProduct);
                    bankContext.SaveChanges();
                }
            }
        }

        public CreditCard GetCreditCardById(string cardId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.CreditCard
                    .FirstOrDefault(_card => _card.Id == cardId);
            }
        }

        public List<CreditCard> GetAllCustomerCreditCards(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var creditCards =  bankContext.CreditCard
                    .Where(card => card.CustomerId == customerId);
                if (creditCards != null)
                {
                    return creditCards.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdateCreditCard(CreditCard creditCard)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.CreditCard.Update(creditCard);
                bankContext.SaveChanges();
            }
        }

        public PrepaidCard GetPrepaidCardById(string cardId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.PrepaidCard
                    .FirstOrDefault(_card => _card.Id == cardId);
            }
        }

        public List<PrepaidCard> GetAllCustomerPrepaidCards(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var prepaidCards =  bankContext.PrepaidCard
                    .Where(card => card.CustomerId == customerId);
                if (prepaidCards != null)
                {
                    return prepaidCards.ToList();
                }
                else
                {
                    return null;
                }
            }
        }

        public void UpdatePrepaidCard(PrepaidCard prepaidCard)
        {
            using (var bankContext = new BankingContext())
            {
                bankContext.PrepaidCard.Update(prepaidCard);
                bankContext.SaveChanges();
            }
        }
    }
}
