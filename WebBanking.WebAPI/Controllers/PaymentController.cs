using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class PaymentController : Controller
    {
        PaymentServices paymentServices;

        public PaymentController()
        {
            var accountServices = new AccountServices();
            var cardServices = new CardServices();
            var loanServices = new LoanServices();
            var transactionServices = new TransactionServices();
            var transferServices = new TransferServices(accountServices, cardServices, loanServices, transactionServices);
            paymentServices = new PaymentServices(transferServices, accountServices, cardServices, loanServices, transactionServices);
        }

        [HttpGet("GetPaymentMethods")]
        public List<PaymentMethod> GetPaymentMethods()
        {
            return paymentServices.GetPaymentMethods();
        }


        [HttpPost("CreditCardPayment")]
        public void CreditCardPayment(TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult;
            transactionResult = paymentServices.AgilePayment(GetCustomerId(), "ΠΛΗΡΩΜΗ ΠΙΣΤΩΤΙΚΗΣ", transaction, language);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        [HttpPost("LoanPayment")]
        public void LoanPayment(TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult;
            transactionResult = paymentServices.AgilePayment(GetCustomerId(), "ΠΛΗΡΩΜΗ ΔΑΝΕΙΟΥ", transaction, language);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        [HttpPost("ThirdPartyPayment")]
        public void ThridPartyPayment(TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult;
            transactionResult = paymentServices.ThirdPartyPayment(GetCustomerId(), transaction, language);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        private string GetCustomerId()
        {
            return (User.Identity as ClaimsIdentity).Claims
                .Where(claim => claim.Type == "custId")
                .FirstOrDefault()
                .Value;
        }

        private void ReturnErrorResponse(string errorMessage)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 400;
            Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorMessage), 0, Encoding.UTF8.GetBytes(errorMessage).Length);
        }
    }
}
