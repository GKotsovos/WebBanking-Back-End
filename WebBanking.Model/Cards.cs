using System.Collections.Generic;

namespace WebBanking.Model
{
    public class Cards
    {
        public List<DebitCardWithLinkedProducts> DebitCards { get; }
        public List<CreditCard> CreditCards { get; }
        public List<PrepaidCard> PrepaidCards { get; }

        public Cards(List<DebitCardWithLinkedProducts> debitCards, List<CreditCard> creditCards, List<PrepaidCard> prepaidCards)
        {
            this.DebitCards = debitCards;
            this.CreditCards = creditCards;
            this.PrepaidCards = prepaidCards;
        }
    }
}
