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

        public AccountOrder GetAccountOrder(string accountId)
        {
            return accountMamager.GetAccountOrderById(accountId);
        }

        public List<AccountOrder> GetAllCustomerAccountOrders(string customerId)
        {
            return accountMamager.GetAllCustomerAccountOrders(customerId).ToList();
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
