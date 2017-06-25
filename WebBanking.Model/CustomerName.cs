using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class CustomerName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CustomerName(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }
    }
}
