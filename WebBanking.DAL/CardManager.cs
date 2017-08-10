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
                return bankContext.LinkedProducts
                    .Where(linkedProduct => linkedProduct.CardId == cardId).ToList();
            }
        }

        public List<DebitCard> GetAllCustomerDebitCards(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.DebitCard
                    .Where(card => card.CustomerId == customerId)
                    .ToList();
            }
        }
        

        public List<LinkedProducts> GetAllCustomerDebitCardsLinkedProducts(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.LinkedProducts
                    .Where(linkedProduct => linkedProduct.CustomerId == customerId)
                    .ToList();
            }
        }

        public void DeleteLinkedProduct(string cardId, string productId)
        {
            using (var bankContext = new BankingContext())
            {
                var linkedProduct = bankContext.LinkedProducts
                    .First(product => product.CardId == cardId && product.ProductId == productId);

                bankContext.Remove(linkedProduct);
                bankContext.SaveChanges();
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
                return bankContext.CreditCard
                    .Where(card => card.CustomerId == customerId)
                    .ToList();
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
                return bankContext.PrepaidCard
                    .Where(card => card.CustomerId == customerId)
                    .ToList();
            }
        }
    }
}
