using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class LoadServices
    {
        AccountServices accountServices;
        CardServices cardServices;
        public LoadServices(AccountServices accountServices, CardServices cardServices)
        {
            this.accountServices = accountServices;
            this.cardServices = cardServices;
        }

        public TransactionResult PrepaidCardLoad(string customerId, CardTransaction cardTransaction, IHasBalances debitAccount)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (debitAccount != null)
            {
                var prepaidCard = cardServices.GetPrePaidCardById(cardTransaction.CardId);
                if (prepaidCard != null)
                {
                    if (debitAccount.AvailableBalance >= (cardTransaction.Amount + cardTransaction.Expenses))
                    {
                        LoadPrepaidCard(prepaidCard, cardTransaction.Amount);
                        accountServices.DebitAccount(debitAccount, cardTransaction.Amount, cardTransaction.Expenses);
                    }
                    else
                    {
                        transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
                    }
                }
                else
                {
                    transactionResult = new TransactionResult(true, "Η προπληρωμένη κάρτα δε βρέθηκε");
                }
            }
            else
            {
                transactionResult = new TransactionResult(true, "Ο λογαριασμός χρέωσης δε βρέθηκε");
            }

            return transactionResult;
        }

        private void LoadPrepaidCard(PrepaidCard prepaidCard, decimal Amount)
        {
            prepaidCard.AvailableBalance += Amount;
            prepaidCard.LedgerBalance += Amount;
        }
    }
}
