using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class OrderManager
    {
        public AccountOrder GetAccountOrderById(string accountOrderId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.AccountOrder
                    .FirstOrDefault(_accountOrder => _accountOrder.Id == accountOrderId);
            }
        }

        public List<AccountOrder> GetAllCustomerAccountOrders(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var accountOrders = bankContext.AccountOrder
                    .Where(accountOrder => accountOrder.CustomerId == customerId)
                    .ToList();

                if (accountOrders != null)
                {
                    return accountOrders;
                }
                else
                {
                    return null;
                }
            }
        }

        public OrganizationOrder GetOrganizationOrderById(string organizationOrderId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.OrganizationOrder
                    .FirstOrDefault(_organizationOrder => _organizationOrder.Id == organizationOrderId);
            }
        }

        public List<OrganizationOrder> GetAllCustomerOrganizationOrders(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var organizationOrders = bankContext.OrganizationOrder
                    .Where(organizationOrder => organizationOrder.CustomerId == customerId)
                    .ToList();

                if (organizationOrders != null)
                {
                    return organizationOrders;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
