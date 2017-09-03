using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class UserManager
    {
        public string GetCustomerId(string userId, string password)
        {
            using (var bankContext = new BankingContext())
            {
                string customerId = "";
                var user =  bankContext.User
                    .FirstOrDefault(usr => usr.Id == userId);

                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        customerId =  user.CustomerId;
                    }
                }

                return customerId;
            }
        }
    }
}
