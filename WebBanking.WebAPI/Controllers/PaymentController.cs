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
        TransactionServices transactionService;
        Helper helper;

        public PaymentController()
        {
            accountServices = new AccountServices();
            cardServices = new CardServices();
            loanServices = new LoanServices();
            paymentServices = new PaymentServices(accountServices, cardServices, loanServices);
            transactionService = new TransactionServices();
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        [HttpGet("GetPaymentMethods")]
        public List<PaymentMethod> GetPaymentMethods()
        {
            return paymentServices.GetPaymentMethods();
        }


        public void Transaction(TransferTransaction transaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");

            IHasBalances debitAccount = helper.GetDebitAccount(transaction.DebitAccount, out transactionResult);
            transactionResult = CheckDebitBalance(debitAccount);
            if (!transactionResult.HasError)
            {
                if (transaction.IsInternal)
                {
                    if (transaction.IsTransfer)
                    {
                        Account creditAccount = helper.GetDebitAccount(transaction.DebitAccount, out transactionResult);
                        transactionResult = transferServices.Transfer(debitAccount, creditAccount, transaction.Amount, transaction.Expenses);
                        //DebitAccount(debitAccount);
                        //CreditAccount(creditAccount);
                        //transactionResult = accountServices.UpdateAccount(creditAccount);
                    }
                    else
                    {
                        IHasInstallment productForPayment = helper.GetProductForPayment(transaction.CreditAccount, out transactionResult);
                        transactionResult = paymentServices.Pay(debitAccount, productForPayment, transaction.Amount, transaction.Expenses);
                        //transactionResult = CheckDebt(productForPayment);
                        //debitAccount(debitAccount);
                        //PayDebt(productForPayment);
                        //transactionResult = helper.UpdateProduct(productForPayment);
                    }
                }
                else
                {
                    DebitAccount(debitAccount);
                }
                transactionResult = Update(debitAccount);
            }
            
            if (transactionResult.HasError)
            {
                transactionService.LogTransaction(GetCustomerId(), transaction, debitAccount.AvailableBalance);
            }
            else
            {
                ReturnErrorResponse(transactionResult.Message);
            }         

        }



        [HttpPost("CreditCardPayment")]
        public void CreditCardPayment(CardTransaction cardTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(cardTransaction.DebitAccountType, cardTransaction.DebitAccount);
            transactionResult = paymentServices.CreditCardPayment(cardTransaction, debitAccount);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                transactionService.AddCreditCardPaymentTransaction(GetCustomerId(), cardTransaction, debitAccount.LedgerBalance);
            }
        }

        [HttpPost("LoanPayment")]
        public void LoanPayment(LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult;
            IHasBalances debitAccount = helper.GetDebitAccount(loanTransaction.DebitAccountType, loanTransaction.DebitAccount);
            transactionResult = paymentServices.LoanPayment(loanTransaction, debitAccount);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            else
            {
                transactionService.AddLoanPaymentTransaction(GetCustomerId(), loanTransaction, debitAccount.LedgerBalance);
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
