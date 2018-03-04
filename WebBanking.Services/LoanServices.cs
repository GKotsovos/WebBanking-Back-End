using System.Collections.Generic;
using System.Linq;
using WebBanking.Model;
using WebBanking.DAL;

namespace WebBanking.Services
{
    public class LoanServices
    {
        LoanManager loanManager;

        public LoanServices()
        {
            loanManager = new LoanManager();
        }

        public Loan GetLoanById(string loanId, out TransactionResult transactionResult, string language)
        {
            transactionResult = new TransactionResult(false, "");
            var loan =  loanManager.GetLoanById(loanId);
            if (loan == null)
            {
                transactionResult.HasError = true;
                transactionResult.Message = language == "greek" ? "Το δάνειο δε βρέθηκε" : "The loan was not found";
            }
            return loan;
        }

        public List<Loan> GetAllCustomerLoans(string customerId)
        {
            return loanManager.GetAllCustomerLoans(customerId).ToList();
        }

        public void LoanUpdate(Loan loan)
        {
            loanManager.Update(loan);
        }
    }
}
