using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class LinkedCardServices
    {
        CardManager cardManager;
        AccountServices accountServices;
        public LinkedCardServices(AccountServices accountServices)
        {
            cardManager = new CardManager();
            this.accountServices = accountServices;
        }

        public DebitCardWithLinkedProducts GetDebitCardWithLinkedProducts(string cardId)
        {
            var debitCard = cardManager.GetDebitCardById(cardId);
            var linkedProducts = cardManager
                .GetDebitCardLinkedProducts(cardId)
                .Select(linkedProduct => accountServices.GetAccountById(linkedProduct.ProductId))
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
                        linkedAccounts.Add(accountServices.GetAccountById(linkedProdct.ProductId));
                    }
                }
                cardWithLinkedProducts.Add(new DebitCardWithLinkedProducts(debitCard, linkedAccounts));
            }

            return cardWithLinkedProducts;
        }
    }
}
