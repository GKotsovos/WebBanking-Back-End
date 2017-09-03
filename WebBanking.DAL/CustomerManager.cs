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
                return bankContext.Customer
                    .FirstOrDefault(_customer => _customer.Id == customerId);
            }
        }
    }
}
