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

        [HttpPost("CreateTransferOrder")]
        public void CreateTransferOrder(TransferOrder transferOrder)
        {
            TransactionResult transactionResult;
            transactionResult = orderServices.CreateTransferOrder(GetCustomerId(), transferOrder);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        [HttpPost("CancelTransferOrder")]
        public void CancelTransferOrder(long transferOrderId)
        {
            TransactionResult transactionResult;
            transactionResult = orderServices.CancelTransferOrder(GetCustomerId(), transferOrderId);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }
        }

        [HttpGet("GetAllCustomerPaymentOrders")]
        public List<PaymentOrder> GetAllCustomerPaymentOrders()
        {
            return orderServices.GetAllCustomerPaymentOrders(GetCustomerId());
        }

        [HttpPost("CreatePaymentOrder")]
        public void CreatePaymentOrder(PaymentOrder paymentOrder)
        {
            TransactionResult transactionResult;
            transactionResult = orderServices.CreatePaymentOrder(GetCustomerId(), paymentOrder);
            if (transactionResult.HasError)
            {
                ReturnErrorResponse(transactionResult.Message);
            }

        }

        [HttpPost("CancelPaymentOrder")]
        public void CancelPaymentOrder(long paymentOrderId)
        {
            TransactionResult transactionResult;
            transactionResult = orderServices.CancelPaymentOrder(GetCustomerId(), paymentOrderId);
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

        private string GetCustomerId()
        {
            return (User.Identity as ClaimsIdentity).Claims
                .Where(claim => claim.Type == "custId")
                .FirstOrDefault()
                .Value;
        }
    }
}
