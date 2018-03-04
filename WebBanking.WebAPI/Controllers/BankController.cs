using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BankController : Controller
    {
        BankServices bankingServices;

        public BankController()
        {
            bankingServices = new BankServices();
        }

        [HttpGet("GetAllDomesticBanks")]
        public List<string> GetAllDomesticBanks()
        {
            return bankingServices.GetAllDomesticBanks();
        }
    }
}
