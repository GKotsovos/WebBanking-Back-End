using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;
using WebBanking.WebAPI.Model;
using System.Security.Claims;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LoadController : Controller
    {
        LoadServices loadServices;
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;
        TransactionServices transactionService;
        Helper helper;

        public LoadController()
        {
            accountServices = new AccountServices();
            cardServices = new CardServices();
            loadServices = new LoadServices(accountServices, cardServices);
            loanServices = new LoanServices();
            transactionService = new TransactionServices();
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        [HttpPost("PrepaidCardLoad")]
        public void PrepaidCardLoad(CardTransaction cardTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(cardTransaction.DebitAccountType, cardTransaction.DebitAccount);
            transactionResult = loadServices.PrepaidCardLoad(cardTransaction, debitAccount);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                transactionService.AddPrepaidCardLoadTransaction(GetCustomerId(), cardTransaction, debitAccount.LedgerBalance);
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
