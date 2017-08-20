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
            return loanServices.GetLoanById(id);
        }

        [HttpGet("GetAllCustomerLoans")]
        public List<Loan> GetAllCustomerLoans()
        {
            //Response.ContentType = "application/json";
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return loanServices.GetAllCustomerLoans(GetCustomerId());
        }

        [HttpPost("LoanPayment")]
        public void LoanPayment(LoanTransaction loanTransaction)
        {
            TransactionResult transactionResult;
            if (loanTransaction.DebitAccountType == "isAccount")
            {
                transactionResult = loanServices.LoanPaymentUsingAccount(GetCustomerId(), loanTransaction);
            }
            else if (loanTransaction.DebitAccountType == "isLoan")
            {
                transactionResult = loanServices.LoanPaymentUsingLoan(GetCustomerId(), loanTransaction);
            }
            else if (loanTransaction.DebitAccountType == "isCreditCard")
            {
                transactionResult = loanServices.LoanPaymentUsingCreditCard(GetCustomerId(), loanTransaction);
            }
            else if (loanTransaction.DebitAccountType == "isPrepaidCard")
            {
                transactionResult = loanServices.LoanPaymentUsingPrepaidCard(GetCustomerId(), loanTransaction);
            }
            else
            {
                transactionResult = new TransactionResult(true, "Τρόπος πληρωμής δε βρέθηκε");
            }

            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        private void ReturnErrorResponse(string errorMessage)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = 400;
            Response.Body.WriteAsync(Encoding.UTF8.GetBytes(errorMessage), 0, Encoding.UTF8.GetBytes(errorMessage).Length);
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
