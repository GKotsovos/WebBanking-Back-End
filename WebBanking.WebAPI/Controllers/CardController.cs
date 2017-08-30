﻿using System;
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
