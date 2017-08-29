﻿using System;
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
    public class PaymentController : Controller
    {
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;
        PaymentServices paymentServices;
        TransactionService transactionService;
        Helper helper;

        public PaymentController()
        {
            accountServices = new AccountServices();
            cardServices = new CardServices();
            loanServices = new LoanServices();
            paymentServices = new PaymentServices(accountServices, cardServices, loanServices);
            transactionService = new TransactionService();
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        [HttpPost("CreditCardPayment")]
        public void CreditCardPayment(CardTransaction cardTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(cardTransaction.DebitAccountType, cardTransaction.DebitAccount);
            transactionResult = paymentServices.CreditCardPayment(GetCustomerId(), cardTransaction, debitAccount);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                transactionService.AddCreditCardPaymentTransaction(GetCustomerId(), cardTransaction, debitAccount.Id, debitAccount.LedgerBalance);
            }
        }

        [HttpPost("LoanPayment")]
        public void LoanPayment(LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(loanTransaction.DebitAccountType, loanTransaction.DebitAccount);
            transactionResult = paymentServices.LoanPayment(GetCustomerId(), loanTransaction, debitAccount);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                transactionService.AddLoanPaymentTransactionHistory(GetCustomerId(), loanTransaction, debitAccount.Id, debitAccount.LedgerBalance);
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
