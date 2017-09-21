using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class OrderServices
    {
        OrderManager accountMamager;

        public OrderServices()
        {
            accountMamager = new OrderManager();
        }

        public TransferOrder GetTransferOrder(long transferOrderId)
        {
            return orderManager.GetTransferOrderById(transferOrderId);
        }

        public List<TransferOrder> GetAllCustomerTransferOrders(string customerId)
        {
            return orderManager.GetAllCustomerTransferOrders(customerId).ToList();
        }

        public OrganizationOrder GetOrganizationOrder(string accountId)
        {
            return accountMamager.GetOrganizationOrderById(accountId);
        }

        public List<OrganizationOrder> GetAllCustomerOrganizationOrders(string customerId)
        {
            return accountMamager.GetAllCustomerOrganizationOrders(customerId).ToList();
        }

    }
}
