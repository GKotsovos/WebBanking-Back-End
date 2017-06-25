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
        TransactionHistoryService transactionHistoryServices;

        public TransactionHistoryController()
        {
            transactionHistoryServices = new TransactionHistoryService();
        }
        
        [HttpGet("GetProductTransactionHistory/{id}")]
        public List<TransactionHistory> GetProductTransactionHistory(string id)
        {
            //Response.ContentType = "application/json";
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return transactionHistoryServices.GetProductTransactionHistory(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
