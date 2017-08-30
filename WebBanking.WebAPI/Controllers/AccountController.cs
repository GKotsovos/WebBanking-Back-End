using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebBanking.Model;
using WebBanking.Services;
using System.Security.Claims;

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
            return accountServices.GetAccountById(id);
        }

        [HttpGet("GetAllCustomerAccounts")]
        public List<Account> GetAllCustomerAccounts()
        {
            return accountServices.GetAllCustomerAccounts(GetCustomerId());
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
