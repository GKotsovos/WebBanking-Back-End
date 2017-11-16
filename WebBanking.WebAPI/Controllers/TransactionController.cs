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
    public class TransactionController : Controller
    {
        TransactionServices transactionServices;

        public TransactionController()
        {
            transactionServices = new TransactionServices();
        }
        
        [HttpGet("GetCurrentMonthProductTransactionHistory/{id}")]
        public List<Transaction> GetCurrentMonthProductTransactionHistory(string id)
        {
            return transactionServices.GetCurrentMonthProductTransactions(id);
        }

        [HttpPost("GetProductTransactionHistoryByTimePeriod")]
        public List<Transaction> GetProductTransactionsByTimePeriod(string productId, DateTime startDate, DateTime endDate)
        {

            return transactionServices.GetProductTransactionsByTimePeriod(productId, startDate, endDate);
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
