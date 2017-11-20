using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class LoadController : Controller
    {
        LoadServices loadServices;

        public LoadController()
        {
            var accountServices = new AccountServices();
            var cardServices = new CardServices();
            var loanServices = new LoanServices();
            var transactionServices = new TransactionServices();
            var transferServices = new TransferServices(accountServices, cardServices, loanServices, transactionServices);
            loadServices = new LoadServices(transferServices, accountServices, cardServices, loanServices, transactionServices);
        }

        [HttpPost("PrepaidCardLoad")]
        public void PrepaidCardLoad(TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult;
            transactionResult = loadServices.PrepaidCardLoad(GetCustomerId(), transaction, language);
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
