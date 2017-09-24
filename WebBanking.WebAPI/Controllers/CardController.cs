using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CardController : Controller
    {
        CardServices cardServices;
        AccountServices accountServices;
        LinkedCardServices linkedCardServices;

        public CardController()
        {
            cardServices = new CardServices();
            accountServices = new AccountServices();
            linkedCardServices = new LinkedCardServices(accountServices);
        }

        [HttpGet("GetDebitCardWithLinkedProductsById/{id}")]
        public DebitCardWithLinkedProducts GetDebitCardWithLinkedProductsById(string id)
        {
            return linkedCardServices.GetDebitCardWithLinkedProducts(id);
        }

        [HttpGet("GetCreditCardById/{id}")]
        public CreditCard GetCreditCardById(string id)
        {
            var transactionResult = new TransactionResult(false, "");
            var creditCard =  cardServices.GetCreditCardById(id, out transactionResult);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            return creditCard;
        }

        [HttpGet("GetPrepaidCardById/{id}")]
        public PrepaidCard GetPrepaidCardById(string id)
        {
            var transactionResult = new TransactionResult(false, "");
            var prepaidCard =  cardServices.GetPrePaidCardById(id, out transactionResult);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            return prepaidCard;
        }

        [HttpGet("GetAllCustomerCards")]
        public Cards GetAllCustomerCards()
        {
            var debitCards = linkedCardServices.GetAllCustomerDebitCardsLinkedProducts(GetCustomerId());
            var creditCards = cardServices.GetAllCustomerCreditCards(GetCustomerId());
            var prepaidCards = cardServices.GetAllCustomerPrepaidCards(GetCustomerId());
            return new Cards(debitCards, creditCards, prepaidCards);
        }

        [HttpGet("GetAllCustomerDebitCardsLinkedProducts")]
        public List<DebitCardWithLinkedProducts> GetAllCustomerDebitCardsLinkedProducts()
        {
            return linkedCardServices.GetAllCustomerDebitCardsLinkedProducts(GetCustomerId());
        }

        [HttpPost("DeleteLinkedProduct")]
        public void DeleteLinkedProduct(string cardId, string productId)
        {
            var transactionResult = new TransactionResult(false, "");
            transactionResult = linkedCardServices.DeleteLinkedProduct(cardId, productId);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        private void ReturnErrorResponse(string errorMessage)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 400;
            Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorMessage), 0, Encoding.UTF8.GetBytes(errorMessage).Length);
        }

        private string GetCustomerId()
        {
            return (User.Identity as ClaimsIdentity).Claims
                .Where(claim => claim.Type == "custId")
                .FirstOrDefault()
                .Value;
        }
    }
}
