using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;

namespace WebBanking.DAL
{
    public class PaymentManager
    {
        public List<PaymentMethod> GetPaymentMethods()
        {
            using (var bankingContext = new BankingContext())
            {
                return bankingContext.PaymentMethod
                    .Select(paymentMethod => paymentMethod)
                    .ToList();
            }
        }
    }
}
