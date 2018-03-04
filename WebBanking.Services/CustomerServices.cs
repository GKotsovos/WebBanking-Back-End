using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class CustomerServices
    {
        CustomerManager customerManager;

        public CustomerServices()
        {
            customerManager = new CustomerManager();
        }

        public CustomerName GetCustomerName(string customerId)
        {
            var customer = customerManager.GetCustomerById(customerId);
            return new CustomerName(customer.Name, customer.LastName);
        }
    }
}
