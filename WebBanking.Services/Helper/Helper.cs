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

        internal IHasBalances GetProduct(string debitProductType, string debitProduct, out TransactionResult transactionResult, string language)
        {
            IHasBalances product = null;
            transactionResult = new TransactionResult(false, "");

            switch (debitProductType)
            {
                case "isAccount":
                    product = accountServices.GetAccountById(debitProduct, out transactionResult, language);
                    break;
                case "isCreditCard":
                    product = cardServices.GetCreditCardById(debitProduct, out transactionResult, language);
                    break;
                case "isPrepaidCard":
                    product = cardServices.GetPrePaidCardById(debitProduct, out transactionResult, language);
                    break;
                case "isLoan":
                    product = loanServices.GetLoanById(debitProduct, out transactionResult, language);
                    break;
            }

            return product;
        }

        internal TransactionResult UpdateProduct(string debitProductType, IHasBalances debitProduct, string language)
        {
            var transactionResult = new TransactionResult(false, "");

            try
            {
                switch (debitProductType)
                {
                    case "isAccount":
                        try
                        {
                            accountServices.UpdateAccount(debitProduct as Account, language);
                        }
                        catch (Exception)
                        {
                            string errorMessage = language == "greek" ? "Λάθος κατά την χρέωση του λογαριασμού" : 
                                "There was a problem during the debit of the account";
                            throw new Exception(errorMessage);
                        }
                        break;
                    case "isCreditCard":
                        try
                        {
                            cardServices.UpdateCreditCard(debitProduct as CreditCard);
                        }
                        catch (Exception)
                        {
                            string errorMessage = language == "greek" ? "Λάθος κατά την χρέωση της πιστωτικής κάρτας" : 
                                "There was a problem during the debit of the credit card";
                            throw new Exception(errorMessage);
                        }
                        break;
                    case "isPrepaidCard":
                        try
                        {
                            cardServices.UpdatePrepaidCard(debitProduct as PrepaidCard, language);
                        }
                        catch (Exception)
                        {
                            string errorMessage = language == "greek" ? "Λάθος κατά την χρέωση της προπληρωμένης κάρτας" : 
                                "There was a problem during the debit of the prepaid card";
                            throw new Exception(errorMessage);
                        }
                        break;
                    case "isLoan":
                        try
                        {
                            loanServices.LoanUpdate(debitProduct as Loan);
                        }
                        catch (Exception)
                        {
                            string errorMessage = language == "greek" ? "Λάθος κατά την χρέωση του δανείου" :
                                "There was a problem during the debit of the loan";
                            throw new Exception(errorMessage);
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
