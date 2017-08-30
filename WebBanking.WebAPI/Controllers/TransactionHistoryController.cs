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
    public class TransactionHistoryController : Controller
    {
        TransactionServices transactionServices;

        public TransactionHistoryController()
        {
            transactionServices = new TransactionServices();
        }
        
        [HttpGet("GetProductTransactionHistory/{id}")]
        public List<Transaction> GetProductTransactionHistory(string id)
        {
            return transactionServices.GetProductTransaction(id);
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
