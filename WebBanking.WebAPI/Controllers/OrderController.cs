using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebBanking.Model;
using WebBanking.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebBanking.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        OrderServices orderServices;

        public OrderController()
        {
            orderServices = new OrderServices();
        }

        [HttpGet("GetAllCustomerTransferOrders")]
        public List<TransferOrder> GetAllCustomerTransferOrders()
        {
            return orderServices.GetAllCustomerTransferOrders(GetCustomerId());
        }

        [HttpGet("GetAllCustomerOrganizationOrders")]
        public List<OrganizationOrder> GetAllCustomerOrganizationOrders()
        {
            return orderServices.GetAllCustomerOrganizationOrders(GetCustomerId());
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
