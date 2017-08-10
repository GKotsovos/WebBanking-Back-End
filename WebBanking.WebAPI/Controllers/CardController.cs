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

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CardController : Controller
    {

        CardServices cardServices;

        public CardController()
        {
            cardServices = new CardServices();
        }

        [HttpGet("GetDebitCardWithLinkedProductsById/{id}")]
        public DebitCardWithLinkedProducts GetDebitCardWithLinkedProductsById(string id)
        {
            return cardServices.GetDebitCardWithLinkedProducts(id);
        }

        [HttpGet("GetCreditCardById/{id}")]
        public CreditCard GetCreditCardById(string id)
        {
            return cardServices.GetCreditCardById(id);
        }

        [HttpGet("GetPrepaidCardById/{id}")]
        public PrepaidCard GetPrepaidCardById(string id)
        {
            return cardServices.GetPrePaidCardById(id);
        }

        [HttpGet("GetAllCustomerCards")]
        public Cards GetAllCustomerCards()
        {
            //Response.ContentType = "application/json";
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            var debitCards = cardServices.GetAllCustomerDebitCardsLinkedProducts(GetCustomerId());
            var creditCards = cardServices.GetAllCustomerCreditCards(GetCustomerId());
            var prepaidCards = cardServices.GetAllCustomerPrepaidCards(GetCustomerId());
            return new Cards(debitCards, creditCards, prepaidCards);
        }

        [HttpGet("GetAllCustomerDebitCardsLinkedProducts")]
        public List<DebitCardWithLinkedProducts> GetAllCustomerDebitCardsLinkedProducts()
        {
            return cardServices.GetAllCustomerDebitCardsLinkedProducts(GetCustomerId());
        }

        [HttpPost("CreditCardPayment")]
        public void CreditCardPayment(CreditCardPaymentDetails creditCardPaymentDetails)
        {
            cardServices.CreditCardPayment(GetCustomerId(), creditCardPaymentDetails);
        }

        // POST api/values
        // DELETE api/values/5
        [HttpPost("DeleteLinkedProduct")]
        public void DeleteLinkedProduct(string cardId, string productId)
        {
            cardServices.DeleteLinkedProduct(cardId, productId);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
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
