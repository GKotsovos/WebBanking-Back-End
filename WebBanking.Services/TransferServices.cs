using WebBanking.Model;
using WebBanking.Services.HelperMethods;

namespace WebBanking.Services
{
    public class TransferServices
    {
        AccountServices accountServices;
        TransactionServices transactionServices;
        Helper helper;

        public TransferServices(AccountServices accountServices, CardServices cardServices, LoanServices loanServices, TransactionServices transactionServices)
        {
            this.accountServices = accountServices;
            this.transactionServices = transactionServices;
            helper = new Helper(accountServices, cardServices, loanServices);
        }

        public TransactionResult CheckDebitBalance(IHasBalances debitProduct, decimal debitAmount, string language)
        {
            TransactionResult transactionResult = new TransactionResult(false, "");
            if (debitProduct.AvailableBalance <= 0 || debitProduct.AvailableBalance < debitAmount)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Λάθος υπόλοιπο λογαριασμού" : "Incorrect account balance";
            }
            return transactionResult;
        }

        public TransactionResult Transfer(string customerId, TransactionDTO transaction, string language)
        {
            var transactionResult = new TransactionResult(false, "");

            IHasBalances debitProduct = helper.GetProduct(transaction.DebitProductType, transaction.DebitProductId, out transactionResult, language);
            transactionResult = CheckDebitBalance(debitProduct, transaction.Amount, language);
            if (!transactionResult.HasError)
            {
                DebitProduct(debitProduct, transaction.Amount, transaction.Expenses);
                transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct, language);
                if (!transactionResult.HasError && transaction.Bank == "AGILGRAA")
                {
                    Account creditProduct = accountServices.GetAccountById(transaction.CreditProductId, out transactionResult, language);
                    if (!transactionResult.HasError)
                    {
                        CreditProduct(creditProduct, transaction.Amount);
                        transactionResult = accountServices.UpdateAccount(creditProduct, language);
                    }
                }
                if (!transactionResult.HasError)
                {
                    transactionResult = helper.UpdateProduct(transaction.DebitProductType, debitProduct, language);
                    if (!transactionResult.HasError)
                    {
                        transactionServices.LogTransaction(customerId, "ΜΕΤΑΦΟΡΑ ΧΡΗΜΑΤΩΝ", debitProduct.AvailableBalance, transaction);
                    }
                }                
            }

            return transactionResult;
        }

        public void DebitProduct(IHasBalances debitProduct, decimal amount, decimal expenses)
        {
            debitProduct.AvailableBalance -= amount + expenses;
            debitProduct.LedgerBalance -= amount + expenses;
        }

        public void CreditProduct(IHasBalances creditProduct, decimal amount)
        {
            creditProduct.AvailableBalance += amount;
            creditProduct.LedgerBalance += amount;
        }
    }
}
