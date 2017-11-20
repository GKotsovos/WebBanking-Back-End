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
        TransactionServices transactionServices;
        Helper helper;
        public LoadServices(TransferServices transferServices, AccountServices accountServices, CardServices cardServices, LoanServices loanServices, TransactionServices transactionServices)
        {
            this.transferServices = transferServices;
            this.accountServices = accountServices;
            this.cardServices = cardServices;
            this.transactionServices = transactionServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public TransactionResult PrepaidCardLoad(string customerId, TransactionDTO transaction, string language)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            IHasBalances debitProduct = helper.GetProduct(transaction.DebitProductType, transaction.DebitProductId, out transactionResult, language);
            transactionResult = transferServices.CheckDebitBalance(debitProduct, transaction.Amount, language);
            if (!transactionResult.HasError)
            {
                var prepaidCard = cardServices.GetPrePaidCardById(transaction.CreditProductId, out transactionResult, language);
                if (!transactionResult.HasError)
                {
                    transferServices.DebitProduct(debitProduct, transaction.Amount, transaction.Expenses);
                    LoadPrepaidCard(prepaidCard, transaction.Amount);
                    transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct, language);
                    if (!transactionResult.HasError)
                    {
                        transactionResult = cardServices.UpdatePrepaidCard(prepaidCard, language);
                        if (!transactionResult.HasError)
                        {
                            transactionServices.LogTransaction(customerId, "ΦΟΡΤΙΣΗ ΠΡΟΠΛ. ΚΑΡΤΑΣ", debitProduct.AvailableBalance, transaction);
                        }
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
