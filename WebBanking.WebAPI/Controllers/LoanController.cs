using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LoanController : Controller
    {
        LoanServices loanServices;

        public LoanController()
        {
            loanServices = new LoanServices();
        }

        [HttpGet("GetLoanById/{id}")]
        public Loan GetLoanById(string id)
        {
            var transactionResult = new TransactionResult(false, "");
            var loan =  loanServices.GetLoanById(id, out transactionResult, "greek");
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
            return loan;
        }

        [HttpGet("GetAllCustomerLoans")]
        public List<Loan> GetAllCustomerLoans()
        {
            return loanServices.GetAllCustomerLoans(GetCustomerId());
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
