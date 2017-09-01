using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.DAL;
using WebBanking.Services.HelperMethods;

namespace WebBanking.Services
{
    public class LoadServices
    {
        TransferServices transferServices;
        AccountServices accountServices;
        CardServices cardServices;
        Helper helper;
        public LoadServices(TransferServices transferServices, AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            this.transferServices = transferServices;
            this.accountServices = accountServices;
            this.cardServices = cardServices;
            this.helper = new Helper(accountServices, cardServices, loanServices);
        }

        public TransactionResult PrepaidCardLoad(TransactionDTO transaction)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            IHasBalances DebitProduct = helper.GetProduct(transaction.DebitProductIdType, transaction.DebitProductId, out transactionResult);
            if (!transactionResult.HasError)
            {
                var prepaidCard = cardServices.GetPrePaidCardById(transaction.CreditProductId, out transactionResult);
                if (!transactionResult.HasError)
                {
                    if (DebitProduct.AvailableBalance >= (transaction.Amount + transaction.Expenses))
                    {
                        LoadPrepaidCard(prepaidCard, transaction.Amount);
                        transferServices.DebitProductId(DebitProduct, transaction.Amount, transaction.Expenses);
                    }
                    else
                    {
                        transactionResult = new TransactionResult(true, "Το υπόλοιπο του λογαριασμού χρέωσης δεν είναι αρκετό");
                    }
                }
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
