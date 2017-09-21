using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class OrderManager
    {
        public TransferOrder GetTransferOrderById(long transferOrderId)
        {
            using (var bankContext = new BankingContext())
            {
                return bankContext.AccountOrder
                    .FirstOrDefault(_accountOrder => _accountOrder.Id == accountOrderId);
            }
        }

        public List<TransferOrder> GetAllCustomerTransferOrders(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var accountOrders = bankContext.AccountOrder
                    .Where(accountOrder => accountOrder.CustomerId == customerId)
                var transferOrders = bankContext.TransferOrder
                    .Where(transferOrder => transferOrder.CustomerId == customerId)
                    .ToList();

                if (transferOrders != null)
                {
                    return transferOrders;
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

                if (paymentOrders != null)
                {
                    return paymentOrders;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
