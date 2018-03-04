using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Text;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TransferController : Controller
    {
        TransferServices transferServices;

        public TransferController()
        {
            transferServices = new TransferServices(new AccountServices(), new CardServices(), new LoanServices(), new TransactionServices());
        }

        [HttpPost("Transfer")]
        public void Transfer(TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult;
            transactionResult = transferServices.Transfer(GetCustomerId(), transaction, language);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
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
