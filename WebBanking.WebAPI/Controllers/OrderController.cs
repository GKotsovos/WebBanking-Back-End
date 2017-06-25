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

        [HttpGet("GetAllCustomerAccountOrders")]
        public List<AccountOrder> GetAllCustomerAccountOrders()
        {
            //Response.ContentType = "application/json";
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return orderServices.GetAllCustomerAccountOrders(GetCustomerId());
        }

        [HttpGet("GetAllCustomerOrganizationOrders")]
        public List<OrganizationOrder> GetAllCustomerOrganizationOrders()
        {
            //Response.ContentType = "application/json";
            //Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            //Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return orderServices.GetAllCustomerOrganizationOrders(GetCustomerId());
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
