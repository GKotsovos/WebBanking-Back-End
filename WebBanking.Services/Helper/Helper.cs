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

        internal IHasBalances GetProduct(string DebitProductType, string DebitProductId, out TransactionResult transactionResult)
        {
            IHasBalances product = null;
            transactionResult = new TransactionResult(false, "");

            switch (DebitProductType)
            {
                case "isAccount":
                    product = accountServices.GetAccountById(DebitProductId, out transactionResult);
                    break;
                case "isCreditCard":
                    product = cardServices.GetCreditCardById(DebitProductId, out transactionResult);
                    break;
                case "isPrepaidCard":
                    product = cardServices.GetPrePaidCardById(DebitProductId, out transactionResult);
                    break;
                case "isLoan":
                    product = loanServices.GetLoanById(DebitProductId, out transactionResult);
                    break;
            }

            return product;
        }

        internal TransactionResult UpdateProduct(string DebitProductType, IHasBalances DebitProduct)
        {
            var transactionResult = new TransactionResult(false, "");

            try
            {
                switch (DebitProductType)
                {
                    case "isAccount":
                        try
                        {
                            accountServices.UpdateAccount(DebitProduct as Account);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση του λογαριασμού");
                        }
                        break;
                    case "isCreditCard":
                        try
                        {
                            cardServices.UpdateCreditCard(DebitProduct as CreditCard);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση της πιστωτικής κάρτας");
                        }
                        break;
                    case "isPrepaidCard":
                        try
                        {
                            cardServices.UpdatePrepaidCard(DebitProduct as PrepaidCard);
                        }
                        catch (Exception)
                        {
                            throw new Exception("Λάθος κατά την χρέωση της προπληρωμένης κάρτας");
                        }
                        break;
                    case "isLoan":
                        try
                        {
                            loanServices.LoanUpdate(DebitProduct as Loan);
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
