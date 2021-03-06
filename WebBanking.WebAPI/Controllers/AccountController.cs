﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebBanking.Model;
using WebBanking.Services;
using System.Security.Claims;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        AccountServices accountServices;

        public AccountController()
        {
            accountServices = new AccountServices();
        }

        [HttpGet("GetAccountById/{id}")]
        public Account GetAccountById(string id)
        {
            var transactionResult = new TransactionResult(false, "");
            var account = accountServices.GetAccountById(id, out transactionResult, "greek");
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            return account;
        }

        [HttpGet("GetAllCustomerAccounts")]
        public List<Account> GetAllCustomerAccounts()
        {
            return accountServices.GetAllCustomerAccounts(GetCustomerId());
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
