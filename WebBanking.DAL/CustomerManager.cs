using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class CustomerManager
    {
        public Customer GetCustomerById(string customerId)
        {
            using (var bankContext = new BankingContext())
            {
                var customer = bankContext.Customer
                    .FirstOrDefault(_customer => _customer.Id == customerId);

                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
