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
    public class TransferController : Controller
    {
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;
        TransferServices transferServices;
        TransactionServices transactionService;
        Helper helper;

        public TransferController()
        {
            accountServices = new AccountServices();
            cardServices = new CardServices();
            loanServices = new LoanServices();
            transferServices = new TransferServices();
            transactionService = new TransactionServices();
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        [HttpPost("Transfer")]
        public void Transfer(TransferTransaction transferTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(transferTransaction.DebitAccountType, transferTransaction.DebitAccount);
            Account creditAccount = accountServices.GetAccountById(transferTransaction.CreditAccount);
            transactionResult = transferServices.Transfer(debitAccount, creditAccount, transferTransaction.Amount, transferTransaction.Expenses);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                helper.UpdateDebitAccount(transferTransaction.DebitAccountType, debitAccount);
                transactionService.AddTransferTransaction(GetCustomerId(), transferTransaction, debitAccount.LedgerBalance);
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
