using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanking.Model;
using WebBanking.Services;

namespace WebBanking.Services.HelperMethods
{
    public class Helper
    {
        AccountServices accountServices;
        CardServices cardServices;
        LoanServices loanServices;

        internal Helper(AccountServices accountServices, CardServices cardServices, LoanServices loanServices)
        {
            this.accountServices = accountServices;
            this.cardServices = cardServices;
            this.loanServices = loanServices;
        }

        internal IHasBalances GetProduct(string debitProductType, string debitProduct, out TransactionResult transactionResult)
        {
            IHasBalances product = null;
            transactionResult = new TransactionResult(false, "");

            switch (debitProductType)
            {
                case "isAccount":
                    product = accountServices.GetAccountById(debitProduct, out transactionResult);
                    break;
                case "isCreditCard":
                    product = cardServices.GetCreditCardById(debitProduct, out transactionResult);
                    break;
                case "isPrepaidCard":
                    product = cardServices.GetPrePaidCardById(debitProduct, out transactionResult);
                    break;
                case "isLoan":
                    product = loanServices.GetLoanById(debitProduct, out transactionResult);
                    break;
            }

            return product;
        }

        internal TransactionResult UpdateProduct(string debitProductType, IHasBalances debitProduct)
        {
            var transactionResult = new TransactionResult(false, "");

            try
            {
                switch (debitProductType)
                {
                    case "isAccount":
                        try
                        {
                            accountServices.UpdateAccount(debitProduct as Account);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση του λογαριασμού");
                        }
                        break;
                    case "isCreditCard":
                        try
                        {
                            cardServices.UpdateCreditCard(debitProduct as CreditCard);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση της πιστωτικής κάρτας");
                        }
                        break;
                    case "isPrepaidCard":
                        try
                        {
                            cardServices.UpdatePrepaidCard(debitProduct as PrepaidCard);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση της προπληρωμένης κάρτας");
                        }
                        break;
                    case "isLoan":
                        try
                        {
                            loanServices.LoanUpdate(debitProduct as Loan);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση του δανείου");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                transactionResult.HasError = true;
                transactionResult.Message = ex.Message;
            }

            return transactionResult;
        }
    }
}
