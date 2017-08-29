using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBanking.Model;
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
