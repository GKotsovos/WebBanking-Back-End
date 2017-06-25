using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBanking.Model
{
    public class DebitCardWithLinkedProducts
    {
        public DebitCard DebitCard { get; set; }
        public List<Account> Accounts { get; set; }

        public DebitCardWithLinkedProducts(DebitCard debitCard, List<Account> accounts)
        {
            this.DebitCard = debitCard;
            this.Accounts = accounts;
        }
    }
}
