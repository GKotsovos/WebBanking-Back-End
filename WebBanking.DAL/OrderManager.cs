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
                var accountOrder = bankContext.AccountOrder
                    .FirstOrDefault(_accountOrder => _accountOrder.Id == accountOrderId);

                if (accountOrder != null)
                {
                    return accountOrder;
                }
                else
                {
                    return null;
                }
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
                var organizationOrder = bankContext.OrganizationOrder
                    .FirstOrDefault(_organizationOrder => _organizationOrder.Id == organizationOrderId);

                if (organizationOrder != null)
                {
                    return organizationOrder;
                }
                else
                {
                    return null;
                }
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
