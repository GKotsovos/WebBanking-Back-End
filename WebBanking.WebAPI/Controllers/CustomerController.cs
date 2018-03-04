using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        CustomerServices customerServices;

        public CustomerController()
        {
            customerServices = new CustomerServices();
        }

        [HttpGet("GetCustomerName")]
        public CustomerName GetCustomerName()
        {
            return customerServices.GetCustomerName(GetCustomerId());
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
