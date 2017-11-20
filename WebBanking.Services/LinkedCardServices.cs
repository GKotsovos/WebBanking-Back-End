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
            var transactionResult = new TransactionResult(false, "");
            var linkedProducts = cardManager
                .GetDebitCardLinkedProducts(cardId)
                .Select(linkedProduct => accountServices.GetAccountById(linkedProduct.ProductId, out transactionResult, "greek"))
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
                        var transactionResult = new TransactionResult(false, "");
                        linkedAccounts.Add(accountServices.GetAccountById(linkedProdct.ProductId, out transactionResult, "greek"));
                    }
                }
                cardWithLinkedProducts.Add(new DebitCardWithLinkedProducts(debitCard, linkedAccounts));
            }

            return cardWithLinkedProducts;
        }

        public TransactionResult DeleteLinkedProduct(string cardId, string productId, string language)
        {
            var transactionResult = new TransactionResult(false, "");
            try
            {
                int linkedProductsCount = cardManager.GetDebitCardLinkedProducts(cardId).Count;
                if (linkedProductsCount != 1)
                {
                    cardManager.DeleteLinkedProduct(cardId, productId);
                }
                else
                {
                    transactionResult.HasError = true;
                    transactionResult.Message = language == "greek" ? "Η κάρτα πρέπει να έχει τουλάχιστον ένα συνδεδεμένο προϊόν" :
                        "The card must have at least one linked product";
                }
            }
            catch (Exception)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Σφάλμα κατά της διαγραφή της σύνδεσης":
                    "There was an error during the deletion of the linkage";
            }            
            return transactionResult;
        }
    }
}
