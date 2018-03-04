using System.Collections.Generic;

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
